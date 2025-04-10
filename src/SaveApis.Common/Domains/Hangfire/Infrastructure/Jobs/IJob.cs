﻿using Hangfire.Server;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;

namespace SaveApis.Common.Domains.Hangfire.Infrastructure.Jobs;

public interface IJob<in TEvent> where TEvent : IEvent
{
    Task<bool> CheckSupportAsync(TEvent @event, CancellationToken cancellationToken);
    Task RunAsync(TEvent @event, PerformContext? performContext, CancellationToken cancellationToken);
}
