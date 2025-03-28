using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Console;
using Hangfire.Pro.Redis;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using SaveApis.Common.Domains.Core.Infrastructure.DI;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;
using SaveApis.Common.Domains.Hangfire.Application.Hangfire.Events;
using SaveApis.Common.Domains.Hangfire.Application.Resolver;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Jobs;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Resolver;

namespace SaveApis.Common.Domains.Hangfire.Application.DI;

public class HangfireModule(IAssemblyHelper assemblyHelper, IConfiguration configuration, HangfireType hangfireType) : BaseModule
{
    private static bool IsConsoleRegistered { get; set; }

    protected override void Load(ContainerBuilder builder)
    {
        if (hangfireType == HangfireType.Unknown)
        {
            return;
        }

        builder.RegisterType<JobConfigurationResolver>().As<IJobConfigurationResolver>();

        RegisterJobs(builder);
        RegisterHangfireServices(builder);

        if (hangfireType != HangfireType.Worker)
        {
            return;
        }

        RegisterHangfireWorker(builder);
    }

    private void RegisterJobs(ContainerBuilder builder)
    {
        var assemblies = assemblyHelper.GetRegisteredAssemblies();

        builder.RegisterAssemblyTypes(assemblies.ToArray())
            .Where(it => it.GetInterfaces().Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IJob<>)))
            .AsImplementedInterfaces()
            .AsSelf();
        builder.RegisterAssemblyOpenGenericTypes(assemblies.ToArray())
            .Where(it => it.GetInterfaces().Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IJob<>)))
            .AsImplementedInterfaces()
            .AsSelf();
    }

    private void RegisterHangfireServices(ContainerBuilder builder)
    {
        var host = configuration["hangfire_redis_host"] ?? "localhost";
        var port = configuration["hangfire_redis_port"] ?? "6379";
        var database = int.TryParse(configuration["hangfire_redis_database"] ?? "0", out var db)
            ? db
            : 0;
        var prefix = configuration["hangfire_redis_prefix"] ?? "hangfire:";
        prefix = prefix.EndsWith(":", StringComparison.InvariantCultureIgnoreCase)
            ? prefix
            : $"{prefix}:";

        var collection = new ServiceCollection();

        collection.AddHangfire((_, globalConfiguration) =>
        {
            var options = new RedisStorageOptions
            {
                Database = database,
                Prefix = prefix,
            };

            globalConfiguration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            globalConfiguration.UseSimpleAssemblyNameTypeSerializer();
            globalConfiguration.UseRecommendedSerializerSettings(settings => settings.Converters.Add(new StringEnumConverter()));
            globalConfiguration.UseRedisStorage($"{host}:{port}", options).WithJobExpirationTimeout(TimeSpan.FromDays(7));
            globalConfiguration.UseThrottling();

            if (IsConsoleRegistered)
            {
                return;
            }

            IsConsoleRegistered = true;
            globalConfiguration.UseConsole();
        });

        builder.Populate(collection);

        builder.RegisterBuildCallback(scope => GlobalConfiguration.Configuration.UseAutofacActivator(scope));
        builder.RegisterBuildCallback(scope =>
        {
            var mediator = scope.Resolve<IMediator>();
            var @event = new ApplicationStartedEvent
            {
                HangfireType = hangfireType,
            };

            mediator.Publish(@event).GetAwaiter().GetResult();
        });
    }

    private void RegisterHangfireWorker(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddHangfireServer((_, options) =>
        {
            options.Queues = Enum.GetNames<HangfireQueue>().Select(it => it.ToLowerInvariant()).ToArray();
            options.WorkerCount = Environment.ProcessorCount * 20;
            options.ServerName = Guid.NewGuid().ToString().Split('-').First();
        });

        builder.Populate(collection);
    }
}
