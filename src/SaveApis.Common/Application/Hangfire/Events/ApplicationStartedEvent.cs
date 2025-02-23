using SaveApis.Common.Domain.Types;
using SaveApis.Common.Infrastructure.Hangfire.Events;

namespace SaveApis.Common.Application.Hangfire.Events;

public class ApplicationStartedEvent : IEvent
{
    public required ApplicationType ApplicationType { get; init; }
}
