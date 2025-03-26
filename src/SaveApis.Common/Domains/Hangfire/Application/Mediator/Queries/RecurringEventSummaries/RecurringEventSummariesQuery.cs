using SaveApis.Common.Domains.Hangfire.Domain.Dtos;
using SaveApis.Common.Domains.Mediator.Infrastructure.Queries;

namespace SaveApis.Common.Domains.Hangfire.Application.Mediator.Queries.RecurringEventSummaries;

public record RecurringEventSummariesQuery : IQuery<IReadOnlyCollection<RecurringEventSummaryDto>>;
