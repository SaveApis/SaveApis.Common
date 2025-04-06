using System.Reflection;
using Hangfire;
using MediatR;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Jobs;
using Serilog;

namespace SaveApis.Common.Domains.Hangfire.Application.Handler;

public class HangfireEventHandler<TEvent>(ILogger logger, IBackgroundJobClientV2 client, IEnumerable<IJob<TEvent>> jobs) : INotificationHandler<TEvent> where TEvent : IEvent
{
    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        logger.Information("Handling event {Event}", notification.GetType().FullName);

        var jobList = jobs.ToList();
        if (jobList.Count == 0)
        {
            logger.Debug("No jobs found for event {Event}", notification.GetType().FullName);

            return;
        }

        foreach (var job in jobList)
        {
            if (!await job.CheckSupportAsync(notification, cancellationToken).ConfigureAwait(false))
            {
                logger.Debug("Job {Job} does not support event {Event}", job.GetType().FullName, notification.GetType().FullName);

                return;
            }

            var queue = ReadJobQueue(job);

            logger.Information("Enqueueing job {Job} to queue {Queue}", job.GetType().FullName, queue.ToString().ToLowerInvariant());
            client.Enqueue(queue.ToString().ToLowerInvariant(), () => job.RunAsync(notification, null, CancellationToken.None));
        }
    }

    private HangfireQueue ReadJobQueue(IJob<TEvent> job)
    {
        var queue = job.GetType().GetCustomAttribute<HangfireQueueAttribute>();
        if (queue is not null)
        {
            return queue.Queue;
        }

        logger.Warning("Job {Job} does not have a queue attribute", job.GetType().FullName);

        return HangfireQueue.Low;
    }
}
