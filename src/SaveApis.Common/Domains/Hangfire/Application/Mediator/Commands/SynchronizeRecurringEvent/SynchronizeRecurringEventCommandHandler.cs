using FluentResults;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.Hangfire.Domain.Dtos;
using SaveApis.Common.Domains.Hangfire.Domain.Entities;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;
using SaveApis.Common.Domains.Hangfire.Persistence.Sql.Factories;
using SaveApis.Common.Domains.Mediator.Infrastructure.Commands;

namespace SaveApis.Common.Domains.Hangfire.Application.Mediator.Commands.SynchronizeRecurringEvent;

public class SynchronizeRecurringEventCommandHandler(IMediator mediator, IRecurringJobManagerV2 manager, IHangfireDbContextFactory factory) : ICommandHandler<SynchronizeRecurringEventCommand>
{
    private readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
    public async Task<Result> Handle(SynchronizeRecurringEventCommand request, CancellationToken cancellationToken)
    {
        var hangfireResult = HandleHangfire(request.Summary);
        var mySqlResult = await HandleMySql(request.Summary).ConfigureAwait(false);

        return Result.Merge(hangfireResult, mySqlResult);
    }

    private Result HandleHangfire(RecurringEventSummaryDto summary)
    {
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

        return Result.Ok();
    }

    private async Task<Result> HandleMySql(RecurringEventSummaryDto summary)
    {
        await using var context = factory.Create();

        switch (summary)
        {
            case { Delete: true, IsInDatabase: true }:
                var existingEntity = await context.RecurringEvents.SingleAsync(it => it.Key.Equals(summary.Key, StringComparison.InvariantCultureIgnoreCase))
                    .ConfigureAwait(false);

                context.RecurringEvents.Remove(existingEntity);
                await context.SaveChangesAsync().ConfigureAwait(false);

                break;
            case { Delete: false, IsInDatabase: false }:
                var newEntity = RecurringEventEntity.Create(Id.Generate(), summary.Key, summary.Cron);

                context.RecurringEvents.Add(newEntity);
                await context.SaveChangesAsync().ConfigureAwait(false);

                break;
            case { Delete: false, IsInDatabase: true, DifferentCron: true }:
                var updatedEntity = await context.RecurringEvents.SingleAsync(it => it.Key.Equals(summary.Key, StringComparison.InvariantCultureIgnoreCase))
                    .ConfigureAwait(false);
                updatedEntity.UpdateCron(summary.Cron);

                await context.SaveChangesAsync().ConfigureAwait(false);

                break;
        }

        return Result.Ok();
    }

    [HangfireJobName("Publish: {0}")]
    public async Task PublishEvent(IEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
    }
}
