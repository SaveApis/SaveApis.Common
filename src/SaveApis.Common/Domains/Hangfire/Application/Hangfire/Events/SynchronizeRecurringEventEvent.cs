using SaveApis.Common.Domains.Hangfire.Domain.Dtos;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;

namespace SaveApis.Common.Domains.Hangfire.Application.Hangfire.Events;

public class SynchronizeRecurringEventEvent : IEvent
{
    public required RecurringEventSummaryDto Summary { get; init; }

    public override string ToString()
    {
        return Summary.Key;
    }
}
