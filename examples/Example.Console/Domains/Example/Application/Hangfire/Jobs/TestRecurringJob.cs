using Example.Console.Domains.Example.Application.Hangfire.Events.Recurring;
using Hangfire.Server;
using MediatR;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Jobs;
using Serilog;
using Serilog.Events;

namespace Example.Console.Domains.Example.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.Medium)]
public class TestRecurringJob(ILogger logger, IMediator mediator) : BaseJob<TestRecurringEvent>(logger)
{
    [HangfireJobName("Test recurring job")]
    public async override Task RunAsync(TestRecurringEvent @event, PerformContext? performContext, CancellationToken cancellationToken)
    {
        Log(LogEventLevel.Information, "Test recurring job", performContext);

        await mediator.Publish(new TestRecurringEventExecutedEvent(), cancellationToken).ConfigureAwait(false);
    }
}
