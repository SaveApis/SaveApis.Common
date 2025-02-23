using Hangfire.Server;
using SaveApis.Common.Infrastructure.Hangfire.Events;

namespace SaveApis.Common.Infrastructure.Hangfire.Jobs;

public interface IJob<in TEvent> where TEvent : IEvent
{
    Task<bool> CheckSupportAsync(TEvent @event, CancellationToken cancellationToken = default);
    Task RunAsync(TEvent @event, PerformContext? performContext = null, CancellationToken cancellationToken = default);
}
