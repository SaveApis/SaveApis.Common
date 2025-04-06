using Hangfire.Server;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SaveApis.Common.Domains.EfCore.Application.Hangfire.Events;
using SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql;
using SaveApis.Common.Domains.Hangfire.Application.Hangfire.Events;
using SaveApis.Common.Domains.Hangfire.Domain.Types;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Jobs;
using Serilog;
using Serilog.Events;

namespace SaveApis.Common.Domains.EfCore.Application.Hangfire.Jobs;

[HangfireQueue(HangfireQueue.System)]
public class MigrateDatabaseJob(ILogger logger, IMediator mediator, IEnumerable<IDesignTimeDbContextFactory<BaseDbContext>> factories) : BaseJob<ApplicationStartedEvent>(logger)
{
    protected override bool CheckSupport(ApplicationStartedEvent @event)
    {
        return @event.HangfireType == HangfireType.Server;
    }

    [HangfireJobName("Migrate Database")]
    public async override Task RunAsync(ApplicationStartedEvent @event, PerformContext? performContext, CancellationToken cancellationToken)
    {
        var factoryList = factories.ToList();
        if (factoryList.Count == 0)
        {
            Log(LogEventLevel.Debug, "No factories found.", performContext);

            return;
        }

        foreach (var factory in factoryList)
        {
            await using var context = factory.CreateDbContext([]);

            Log(LogEventLevel.Information, $"Migrate context {context.GetType().Name}", performContext);
            await context.Database.MigrateAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            Log(LogEventLevel.Information, $"Migrate context {context.GetType().Name} - Done", performContext);
        }

        await mediator.Publish(new MigrationCompletedEvent(), cancellationToken).ConfigureAwait(false);
    }
}
