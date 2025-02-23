using Hangfire.Console;
using Hangfire.Server;
using Hangfire.Throttling;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SaveApis.Common.Application.Hangfire.Events;
using SaveApis.Common.Infrastructure.Hangfire.Attributes;
using SaveApis.Common.Infrastructure.Hangfire.Jobs;
using SaveApis.Common.Infrastructure.Persistence.Sql;

namespace SaveApis.Common.Application.Hangfire.Jobs;

[Mutex("core:database:migrate")]
[HangfireQueue(HangfireQueue.System)]
public class MigrateDatabaseJob(IMediator mediator, IEnumerable<IDesignTimeDbContextFactory<BaseDbContext>> registeredFactories) : BaseJob<ApplicationStartedEvent>
{
    [HangfireJobName("Migrate databases")]
    public async override Task RunAsync(ApplicationStartedEvent @event, PerformContext? performContext = null, CancellationToken cancellationToken = default)
    {
        var bar = performContext.WriteProgressBar("Migrating database");
        var factories = registeredFactories.ToList();

        foreach (var factory in factories.WithProgress(bar))
        {
            await using var context = factory.CreateDbContext([]);

            performContext.WriteLine($"Migrate {context.GetType().Name}");
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            performContext.WriteLine($"Migrate {context.GetType().Name} - Done");

            var completedEvent = new MigrationCompletedEvent
            {
                Type = context.GetType(),
            };

            await mediator.Publish(completedEvent, cancellationToken).ConfigureAwait(false);
        }
    }
}
