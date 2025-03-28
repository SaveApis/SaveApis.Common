using SaveApis.Common.Domains.Hangfire.Infrastructure.Configurations;

namespace Example.Console.Domains.Example.Application.Hangfire.Configuration;

public class TestConfiguration : IJobConfiguration
{
    public bool Active { get; set; }
}
