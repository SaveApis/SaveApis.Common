using FluentResults;
using MediatR;
using SaveApis.Common.Domains.Hangfire.Application.Mediator.Queries.RecurringEvents;
using SaveApis.Common.Domains.Hangfire.Domain.Dtos;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Mediator.Infrastructure.Queries;

namespace SaveApis.Common.Domains.Hangfire.Application.Mediator.Queries.RecurringEventSummaries;

public class RecurringEventSummariesQueryHandler(IMediator mediator) : IQueryHandler<RecurringEventSummariesQuery, IReadOnlyCollection<RecurringEventSummaryDto>>
{
    public async Task<Result<IReadOnlyCollection<RecurringEventSummaryDto>>> Handle(RecurringEventSummariesQuery request, CancellationToken cancellationToken)
    {
        var hangfireResult = await mediator.Send(new RecurringEventsQuery(RecurringEventSource.Hangfire), cancellationToken).ConfigureAwait(false);
        if (hangfireResult.IsFailed)
        {
            return Result.Fail("Failed to get Hangfire-based recurring events").WithErrors(hangfireResult.Errors);
        }

        var codeResult = await mediator.Send(new RecurringEventsQuery(RecurringEventSource.Code), cancellationToken).ConfigureAwait(false);
        if (codeResult.IsFailed)
        {
            return Result.Fail("Failed to get code-based recurring events").WithErrors(codeResult.Errors);
        }

        var summaries = new List<RecurringEventSummaryDto>();
        summaries = Enrich(summaries, hangfireResult.Value, dto => dto.IsInHangfire = true);
        summaries = Enrich(summaries, codeResult.Value, dto => dto.IsInCode = true);

        return summaries;
    }

    private static List<RecurringEventSummaryDto> Enrich(List<RecurringEventSummaryDto> dtos, IReadOnlyCollection<RecurringEventGetDto> events, Action<RecurringEventSummaryDto> enrichAction)
    {
        var internalList = new List<RecurringEventSummaryDto>();
        internalList.AddRange(dtos.Where(dto => !events.Any(@event => @event.Key.Equals(dto.Key, StringComparison.InvariantCultureIgnoreCase))));

        foreach (var @event in events)
        {
            var dto = dtos.SingleOrDefault(summaryDto => summaryDto.Key.Equals(@event.Key, StringComparison.InvariantCultureIgnoreCase));
            dto ??= new RecurringEventSummaryDto
            {
                Key = @event.Key,
                Cron = @event.Cron,
            };

            enrichAction(dto);
            if (!dto.Cron.Equals(@event.Cron, StringComparison.InvariantCultureIgnoreCase))
            {
                dto.DifferentCron = true;
            }
            dto.Cron = @event.Cron;
            if (@event.Event is not null)
            {
                dto.Event = @event.Event;
            }

            internalList.Add(dto);
        }

        return internalList.OrderBy(dto => dto.Key).ToList();
    }
}
