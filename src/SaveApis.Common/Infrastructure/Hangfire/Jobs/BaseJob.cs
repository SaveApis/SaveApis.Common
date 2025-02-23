using Hangfire.Server;
using SaveApis.Common.Infrastructure.Hangfire.Events;

namespace SaveApis.Common.Infrastructure.Hangfire.Jobs;

public abstract class BaseJob<TEvent> : IJob<TEvent> where TEvent : IEvent
{
    protected virtual bool CheckSupport(TEvent @event)
    {
        return true;
    }

    public virtual Task<bool> CheckSupportAsync(TEvent @event, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(CheckSupport(@event));
    }

    public abstract Task RunAsync(TEvent @event, PerformContext? performContext = null, CancellationToken cancellationToken = default);
}
