using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;

namespace SaveApis.Common.Domains.Hangfire.Application.Hangfire.Events;

public class ApplicationStartedEvent : IEvent
{
    public required HangfireType HangfireType { get; init; }
}
