using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.Hangfire.Domain.Entities;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Configurations;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Resolver;
using SaveApis.Common.Domains.Hangfire.Persistence.Sql.Factories;
using Serilog;

namespace SaveApis.Common.Domains.Hangfire.Application.Resolver;

public class JobConfigurationResolver(ILogger logger, IHangfireDbContextFactory factory) : IJobConfigurationResolver
{
    public async Task<TJobConfiguration> Resolve<TJobConfiguration>() where TJobConfiguration : class, IJobConfiguration
    {
        var resolvedConfiguration = await ResolveInternal(typeof(TJobConfiguration)).ConfigureAwait(false);
        if (resolvedConfiguration is not TJobConfiguration configuration)
        {
            throw new InvalidOperationException($"No job configuration found for type {typeof(TJobConfiguration).Name}");
        }

        if (configuration is null)
        {
            throw new InvalidOperationException($"No job configuration found for type {typeof(TJobConfiguration).Name}");
        }

        return configuration;
    }

    public async Task<IJobConfiguration> Resolve(Type jobConfigurationType)
    {
        var resolvedConfiguration = await ResolveInternal(jobConfigurationType).ConfigureAwait(false);
        if (resolvedConfiguration is not IJobConfiguration jobConfiguration)
        {
            throw new InvalidOperationException($"No job configuration found for type {jobConfigurationType.Name}");
        }

        return jobConfiguration;
    }

    private async Task<object?> ResolveInternal(Type jobConfigurationType)
    {
        var configurationNamespace = ReadConfigurationNamespace(jobConfigurationType);
        var instance = Activator.CreateInstance(jobConfigurationType);

        try
        {
            await using var context = factory.Create();

            var jobConfigurationEntries = await context.JobConfigurations
                .Where(jc => jc.Namespace == configurationNamespace)
                .ToListAsync()
                .ConfigureAwait(false);

            foreach (var property in jobConfigurationType.GetProperties())
            {
                var entry = jobConfigurationEntries.SingleOrDefault(jc => jc.Name == property.Name);
                if (entry is null)
                {
                    var serializedValue = JsonConvert.SerializeObject(property.GetValue(instance));
                    entry = JobConfigurationEntity.Create(Id.From(Guid.NewGuid()), configurationNamespace, property.Name, serializedValue);
                    context.JobConfigurations.Add(entry);

                    continue;
                }

                var deserializedValue = JsonConvert.DeserializeObject(entry.Value, property.PropertyType);
                property.SetValue(instance, deserializedValue);
            }

            await context.SaveChangesAsync().ConfigureAwait(false);

            return instance;
        }
        catch (Exception e)
        {
            logger.Error(e, "Error resolving job configuration {JobConfigurationType}", jobConfigurationType.Name);
        }

        return instance;
    }
    private static string ReadConfigurationNamespace(Type jobConfigurationType)
    {
        var @namespace = $"{jobConfigurationType.Name[0].ToString().ToLower()}{jobConfigurationType.Name[1..]}".Replace("Configuration", string.Empty);
        if (jobConfigurationType.BaseType is not null && jobConfigurationType.BaseType != typeof(object))
        {
            @namespace = $"{ReadConfigurationNamespace(jobConfigurationType.BaseType)}.{@namespace}";
        }

        return @namespace;
    }
}
