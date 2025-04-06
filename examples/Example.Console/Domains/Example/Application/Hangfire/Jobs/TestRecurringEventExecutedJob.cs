using Example.Console.Domains.Example.Application.Hangfire.Events.Recurring;
using Hangfire.Server;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Jobs;
using Serilog;
using Serilog.Events;

namespace Example.Console.Domains.Example.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.Low)]
public class TestRecurringEventExecutedJob(ILogger logger) : BaseJob<TestRecurringEventExecutedEvent>(logger)
{
    [HangfireJobName("Test recurring event executed")]
    public override Task RunAsync(TestRecurringEventExecutedEvent @event, PerformContext? performContext, CancellationToken cancellationToken)
    {
        Log(LogEventLevel.Information, "Test recurring job executed", performContext);

        return Task.CompletedTask;
    }
}
