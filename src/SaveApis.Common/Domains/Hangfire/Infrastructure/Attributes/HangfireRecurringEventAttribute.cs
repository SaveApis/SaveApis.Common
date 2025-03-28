namespace SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class HangfireRecurringEventAttribute(string key, string cron) : Attribute
{
    public string Key { get; } = key;
    public string Cron { get; } = cron;
}
