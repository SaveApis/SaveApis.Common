using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;

namespace SaveApis.Common.Domains.Hangfire.Domain.Dtos;

public class RecurringEventSummaryDto
{
    public required string Key { get; init; }
    public required string Cron { get; set; }
    public IEvent? Event { get; set; }

    public bool IsInCode { get; set; }
    public bool IsInDatabase { get; set; }
    public bool IsInHangfire { get; set; }
    public bool DifferentCron { get; set; }

    public bool IsInSync => IsInCode && IsInDatabase && IsInHangfire && !DifferentCron;
    public bool Delete => !IsInCode && (IsInDatabase || IsInHangfire);
}
