using MediatR;
using Microsoft.EntityFrameworkCore;
using SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql;
using SaveApis.Common.Domains.Hangfire.Domain.Entities;
using SaveApis.Common.Domains.Hangfire.Persistence.Sql.Configurations;

namespace SaveApis.Common.Domains.Hangfire.Persistence.Sql;

public class HangfireDbContext(DbContextOptions options, IMediator mediator) : BaseDbContext(options, mediator)
{
    protected override string Schema => "Hangfire";

    public DbSet<JobConfigurationEntity> JobConfigurations { get; set; }
    public DbSet<RecurringEventEntity> RecurringEvents { get; set; }

    protected override void CreateEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new JobConfigurationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RecurringEventEntityConfiguration());
    }
}
