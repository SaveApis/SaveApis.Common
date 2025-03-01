using SaveApis.Common.Infrastructure.Hangfire.Events;

namespace SaveApis.Common.Application.Hangfire.Events;

public class MigrationCompletedEvent : IEvent
{
    public required Type Type { get; init; }
}
