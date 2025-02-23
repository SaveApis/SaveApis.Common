using System.Reflection;
using Hangfire;
using MediatR;
using SaveApis.Common.Infrastructure.Hangfire.Attributes;
using SaveApis.Common.Infrastructure.Hangfire.Events;
using SaveApis.Common.Infrastructure.Hangfire.Jobs;
using Serilog;

namespace SaveApis.Common.Application.Hangfire.Handler;

public class HangfireEventHandler<TEvent>(IBackgroundJobClientV2 client, ILogger logger, IEnumerable<IJob<TEvent>> jobs) : INotificationHandler<TEvent> where TEvent : IEvent
{
    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        var assignedJobs = jobs.ToList();
        logger.Information("Recieved event {Name} ({Count} Jobs)", typeof(TEvent).FullName, assignedJobs.Count);

        foreach (var job in assignedJobs)
        {
            if (!await job.CheckSupportAsync(notification, cancellationToken).ConfigureAwait(false))
            {
                logger.Information("{JobName} canceled. Event not supported. ({EventName})", job.GetType().FullName, typeof(TEvent).FullName);
                continue;
            }

            var queue = ReadQueue(job) ?? HangfireQueue.Medium;

            logger.Information("Enqueueing job {JobName} ({Queue})", job.GetType().FullName, queue);
            client.Enqueue(queue.ToString().ToLowerInvariant(), () => job.RunAsync(notification, null, CancellationToken.None));
        }
    }

    private HangfireQueue? ReadQueue(IJob<TEvent> job)
    {
        var queueAttribute = job.GetType().GetCustomAttribute<HangfireQueueAttribute>();
        if (queueAttribute is not null)
        {
            return queueAttribute.Queue;
        }

        logger.Warning("No queue attribute found for {JobName}", job.GetType().FullName);

        return null;
    }
}
