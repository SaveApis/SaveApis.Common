namespace SaveApis.Common.Infrastructure.Hangfire.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class HangfireRecurringEventAttribute(string id, string cron) : Attribute
{
    public string Id { get; } = id;
    public string Cron { get; } = cron;
}
