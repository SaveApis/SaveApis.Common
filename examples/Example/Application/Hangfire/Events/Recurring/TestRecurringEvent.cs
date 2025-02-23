using SaveApis.Common.Infrastructure.Hangfire.Attributes;
using SaveApis.Common.Infrastructure.Hangfire.Events;

namespace Example.Application.Hangfire.Events.Recurring;

[HangfireRecurringEvent("test", "* * * * *")]
public class TestRecurringEvent : IEvent;
