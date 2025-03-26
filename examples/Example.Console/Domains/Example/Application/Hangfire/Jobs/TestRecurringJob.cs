using Example.Console.Domains.Example.Application.Hangfire.Configuration;
using Example.Console.Domains.Example.Application.Hangfire.Events.Recurring;
using Hangfire.Server;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Jobs;
using Serilog;
using Serilog.Events;

namespace Example.Console.Domains.Example.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.Medium)]
public class TestRecurringJob(ILogger logger) : BaseJob<TestRecurringEvent, TestConfiguration>(logger)
{
    [HangfireJobName("Test recurring job")]
    public override Task RunAsync(TestRecurringEvent @event, PerformContext? performContext, CancellationToken cancellationToken)
    {
        Log(LogEventLevel.Information, "Test recurring job", performContext);

        return Task.CompletedTask;
    }
}
