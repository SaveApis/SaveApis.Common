using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;

namespace Example.Console.Domains.Example.Application.Hangfire.Events.Recurring;

[HangfireRecurringEvent("test", "* * * * *")]
public class TestRecurringEvent : IEvent;
