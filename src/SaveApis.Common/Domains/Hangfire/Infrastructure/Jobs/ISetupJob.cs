using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;

namespace SaveApis.Common.Domains.Hangfire.Infrastructure.Jobs;

public interface ISetupJob<in TEvent> : IJob<TEvent> where TEvent : IEvent;
