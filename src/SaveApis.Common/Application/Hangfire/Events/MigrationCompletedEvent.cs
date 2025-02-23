namespace SaveApis.Common.Application.Hangfire.Events;

public class MigrationCompletedEvent
{
    public required Type Type { get; init; }
}
