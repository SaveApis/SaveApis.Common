using System.Reflection;
using Hangfire;
using MediatR;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Jobs;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Resolver;
using Serilog;

namespace SaveApis.Common.Domains.Hangfire.Application.Handler;

public class HangfireEventHandler<TEvent>(ILogger logger, IJobConfigurationResolver resolver, IBackgroundJobClientV2 client, IEnumerable<IJob<TEvent>> jobs) : INotificationHandler<TEvent> where TEvent : IEvent
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
            if (job is ISetupJob<TEvent> setupJob)
            {
                await HandleInternalAsync(setupJob, notification, cancellationToken).ConfigureAwait(false);

                continue;
            }

            var configurationType = ResolveJobConfigurationType(job);
            var configuration = await resolver.Resolve(configurationType).ConfigureAwait(false);
            if (!configuration.Active)
            {
                logger.Information("Job {Job} is not active", job.GetType().FullName);

                return;
            }

            job.ApplyConfiguration(configuration);

            await HandleInternalAsync(job, notification, cancellationToken).ConfigureAwait(false);
        }
    }

    private static Type ResolveJobConfigurationType(IJob<TEvent> job)
    {
        var currentType = job.GetType();
        while (currentType.BaseType is not null && currentType.BaseType != typeof(object))
        {
            if (currentType.BaseType.IsGenericType && currentType.BaseType.GetGenericTypeDefinition() == typeof(BaseJob<,>))
            {
                return currentType.BaseType.GenericTypeArguments[1];
            }

            currentType = currentType.BaseType;
        }

        throw new InvalidOperationException("Job does not have a configuration");
    }

    private async Task HandleInternalAsync(IJob<TEvent> job, TEvent notification, CancellationToken cancellationToken)
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
