using SaveApis.Common.Domains.Hangfire.Domain.Types;

namespace SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class HangfireQueueAttribute(HangfireQueue queue) : Attribute
{
    public HangfireQueue Queue { get; } = queue;
}
