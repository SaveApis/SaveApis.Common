using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Console;
using Hangfire.Pro.Redis;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using SaveApis.Common.Application.Hangfire;
using SaveApis.Common.Application.Hangfire.Events;
using SaveApis.Common.Domain.Types;
using SaveApis.Common.Infrastructure.DI;
using SaveApis.Common.Infrastructure.Hangfire.Jobs;
using SaveApis.Common.Infrastructure.Helper;

namespace SaveApis.Common.Application.DI;

public class HangfireModule(IConfiguration configuration, IAssemblyHelper helper, ApplicationType applicationType) : BaseModule
{
    private static bool IsConsoleRegistered { get; set; }

    protected override void Load(ContainerBuilder builder)
    {
        RegisterJobs(builder);
        RegisterHangfire(builder);

        if (applicationType == ApplicationType.Worker)
        {
            RegisterWorker(builder);
        }

        builder.RegisterBuildCallback(scope =>
        {
            var @event = new ApplicationStartedEvent
            {
                ApplicationType = applicationType,
            };

            var mediator = scope.Resolve<IMediator>();
            mediator.Publish(@event).GetAwaiter().GetResult();
        });
    }

    private void RegisterJobs(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(helper.GetAssemblies().ToArray())
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IJob<>)))
            .AsImplementedInterfaces()
            .AsSelf();
    }

    private void RegisterHangfire(ContainerBuilder builder)
    {
        var host = configuration["hangfire_redis_host"] ?? string.Empty;
        var port = configuration["hangfire_redis_port"] ?? string.Empty;
        var database = configuration["hangfire_redis_database"] ?? string.Empty;
        var prefix = configuration["hangfire_redis_prefix"] ?? string.Empty;
        prefix = prefix.EndsWith(':')
            ? prefix
            : $"{prefix}:";

        var redisOptions = new RedisStorageOptions
        {
            Database = int.TryParse(database, out var db)
                ? db
                : 6979,
            Prefix = prefix,
        };

        var collection = new ServiceCollection();

        collection.AddHangfire((_, globalConfiguration) =>
        {
            globalConfiguration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            globalConfiguration.UseSimpleAssemblyNameTypeSerializer();
            globalConfiguration.UseRecommendedSerializerSettings(settings => settings.Converters.Add(new StringEnumConverter()));
            globalConfiguration.UseRedisStorage($"{host}:{port}", redisOptions).WithJobExpirationTimeout(TimeSpan.FromDays(7));

            if (!IsConsoleRegistered)
            {
                globalConfiguration.UseConsole();
                IsConsoleRegistered = true;
            }

            globalConfiguration.UseSerilogLogProvider();
            globalConfiguration.UseThrottling();
        });

        builder.Populate(collection);

        builder.RegisterBuildCallback(scope => GlobalConfiguration.Configuration.UseAutofacActivator(scope));
    }

    private static void RegisterWorker(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddHangfireServer((_, options) =>
        {
            options.ServerName = Guid.NewGuid().ToString()[..8];
            options.WorkerCount = Environment.ProcessorCount * 20;
            options.Queues = Enum.GetValues<HangfireQueue>().Select(it => it.ToString().ToLowerInvariant()).ToArray();
        });

        builder.Populate(collection);
    }
}
