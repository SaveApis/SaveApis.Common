using SaveApis.Common.Domains.Hangfire.Infrastructure.Configurations;

namespace SaveApis.Common.Domains.Hangfire.Infrastructure.Resolver;

public interface IJobConfigurationResolver
{
    Task<TJobConfiguration> Resolve<TJobConfiguration>() where TJobConfiguration : class, IJobConfiguration;
    Task<IJobConfiguration> Resolve(Type jobConfigurationType);
}
