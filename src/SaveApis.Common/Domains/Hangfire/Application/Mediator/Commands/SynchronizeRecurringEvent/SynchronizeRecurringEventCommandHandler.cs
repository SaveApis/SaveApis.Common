using FluentResults;
using Hangfire;
using MediatR;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;
using SaveApis.Common.Domains.Mediator.Infrastructure.Commands;

namespace SaveApis.Common.Domains.Hangfire.Application.Mediator.Commands.SynchronizeRecurringEvent;

public class SynchronizeRecurringEventCommandHandler(IMediator mediator, IRecurringJobManagerV2 manager) : ICommandHandler<SynchronizeRecurringEventCommand>
{
    private readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
    public Task<Result> Handle(SynchronizeRecurringEventCommand request, CancellationToken cancellationToken)
    {
        var summary = request.Summary;
        switch (summary)
        {
            case { Delete: true, IsInHangfire: true }:
                manager.RemoveIfExists(summary.Key);

                break;
            case { Delete: false, IsInHangfire: false }:
            case { Delete: false, IsInHangfire: true, DifferentCron: true }:
                if (summary.Event is null)
                {
                    throw new ArgumentNullException(nameof(summary.Event), "Event cannot be null when adding or updating a recurring job.");
                }

                var options = new RecurringJobOptions
                {
                    TimeZone = _timeZone,
                };
                manager.AddOrUpdate(summary.Key, nameof(HangfireQueue.Event).ToLowerInvariant(), () => PublishEvent(summary.Event, CancellationToken.None), () => summary.Cron, options);

                break;
        }

        return Task.FromResult(Result.Ok());
    }

    [HangfireJobName("Publish: {0}")]
    public async Task PublishEvent(IEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
    }
}
