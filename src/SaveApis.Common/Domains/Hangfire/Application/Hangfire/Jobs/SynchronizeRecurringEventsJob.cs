using Hangfire.Server;
using MediatR;
using SaveApis.Common.Domains.EfCore.Application.Hangfire.Events;
using SaveApis.Common.Domains.Hangfire.Application.Hangfire.Events;
using SaveApis.Common.Domains.Hangfire.Application.Mediator.Queries.RecurringEventSummaries;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Jobs;
using Serilog;
using Serilog.Events;

namespace SaveApis.Common.Domains.Hangfire.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.System)]
public class SynchronizeRecurringEventsJob(ILogger logger, IMediator mediator) : BaseSetupJob<MigrationCompletedEvent>(logger)
{
    [HangfireJobName("Synchronize recurring events")]
    public async override Task RunAsync(MigrationCompletedEvent @event, PerformContext? performContext, CancellationToken cancellationToken)
    {
        var queryResult = await mediator.Send(new RecurringEventSummariesQuery(), cancellationToken).ConfigureAwait(false);
        if (queryResult.IsFailed)
        {
            var errorString = string.Join(", ", queryResult.Errors.Select(it => it.Message));
            Log(LogEventLevel.Error, $"Failed to generate recurring event summary! ({errorString})", performContext);

            return;
        }

        var notSyncedSummaries = queryResult.Value.Where(dto => !dto.IsInSync).ToList();
        if (notSyncedSummaries.Count == 0)
        {
            Log(LogEventLevel.Information, "No recurring events to synchronize", performContext);

            return;
        }

        foreach (var summary in notSyncedSummaries)
        {
            var synchronizeEvent = new SynchronizeRecurringEventEvent
            {
                Summary = summary,
            };
            await mediator.Publish(synchronizeEvent, cancellationToken).ConfigureAwait(false);
        }
    }
}
