using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;

namespace SaveApis.Common.Domains.Hangfire.Domain.Dtos;

public class RecurringEventGetDto
{
    public required string Key { get; init; }
    public required string Cron { get; set; }
    public IEvent? Event { get; set; }
}
