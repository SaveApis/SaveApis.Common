namespace SaveApis.Common.Domains.Hangfire.Domain.Types;

public enum HangfireQueue
{
    /// <summary>
    /// This is only used to execute recurring events
    /// </summary>
    Event,
    System,
    High,
    Medium,
    Low,
}
