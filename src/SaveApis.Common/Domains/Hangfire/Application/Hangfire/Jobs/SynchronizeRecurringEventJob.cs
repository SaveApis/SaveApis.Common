using Hangfire.Server;
using MediatR;
using SaveApis.Common.Domains.Hangfire.Application.Hangfire.Events;
using SaveApis.Common.Domains.Hangfire.Application.Mediator.Commands.SynchronizeRecurringEvent;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Jobs;
using Serilog;
using Serilog.Events;

namespace SaveApis.Common.Domains.Hangfire.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.System)]
public class SynchronizeRecurringEventJob(ILogger logger, IMediator mediator) : BaseSetupJob<SynchronizeRecurringEventEvent>(logger)
{
    [HangfireJobName("{0}: Synchronize recurring event")]
    public async override Task RunAsync(SynchronizeRecurringEventEvent @event, PerformContext? performContext, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new SynchronizeRecurringEventCommand(@event.Summary), cancellationToken).ConfigureAwait(false);
        if (result.IsFailed)
        {
            var errorString = string.Join(", ", result.Errors.Select(it => it.Message));
            Log(LogEventLevel.Error, $"Failed to synchronize recurring event! ({errorString})", performContext);
        }
    }
}
