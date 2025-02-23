using System.Reflection;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Hangfire.Throttling;
using MediatR;
using SaveApis.Common.Application.Hangfire.Events;
using SaveApis.Common.Domain.Types;
using SaveApis.Common.Infrastructure.Hangfire.Attributes;
using SaveApis.Common.Infrastructure.Hangfire.Events;
using SaveApis.Common.Infrastructure.Hangfire.Jobs;
using SaveApis.Common.Infrastructure.Helper;

namespace SaveApis.Common.Application.Hangfire.Jobs;

[Mutex("core:hangfire:register:recurring-events")]
[HangfireQueue(HangfireQueue.System)]
public class RegisterRecurringEventsJob(ITypeHelper helper, IMediator mediator, IRecurringJobManagerV2 manager) : BaseJob<ApplicationStartedEvent>
{
    protected override bool CheckSupport(ApplicationStartedEvent @event)
    {
        return @event.ApplicationType == ApplicationType.Server;
    }

    [HangfireJobName("Register recurring events")]
    public override Task RunAsync(ApplicationStartedEvent @event, PerformContext? performContext = null, CancellationToken cancellationToken = default)
    {
        var options = new RecurringJobOptions
        {
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time"),
        };

        var types = helper
            .ByAttribute<HangfireRecurringEventAttribute>()
            .ToList();
        performContext.WriteLine($"Found {types.Count} recurring events");

        foreach (var type in types)
        {
            var instance = (IEvent)Activator.CreateInstance(type)!;
            var attribute = type.GetCustomAttribute<HangfireRecurringEventAttribute>()!;

            performContext.WriteLine($"Registering {attribute.Id} ({attribute.Cron}");
            manager.AddOrUpdate(attribute.Id, nameof(HangfireQueue.RecurringEvent).ToLowerInvariant(), () => PublishEvents(instance), () => attribute.Cron, options);
        }

        return Task.CompletedTask;
    }

    [HangfireJobName("Event: {0}")]
    public async Task PublishEvents(IEvent @event)
    {
        await mediator.Publish(@event).ConfigureAwait(false);
    }
}
