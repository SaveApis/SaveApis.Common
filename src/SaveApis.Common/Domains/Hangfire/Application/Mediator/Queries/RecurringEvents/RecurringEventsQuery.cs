using SaveApis.Common.Domains.Hangfire.Domain.Dtos;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Mediator.Infrastructure.Queries;

namespace SaveApis.Common.Domains.Hangfire.Application.Mediator.Queries.RecurringEvents;

public record RecurringEventsQuery(RecurringEventSource Source) : IQuery<IReadOnlyCollection<RecurringEventGetDto>>;
