using SaveApis.Common.Application.Hangfire;

namespace SaveApis.Common.Infrastructure.Hangfire.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class HangfireQueueAttribute(HangfireQueue queue) : Attribute
{
    public HangfireQueue Queue { get; } = queue;
}
