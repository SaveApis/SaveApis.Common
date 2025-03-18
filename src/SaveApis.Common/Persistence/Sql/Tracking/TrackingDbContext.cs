using MediatR;
using Microsoft.EntityFrameworkCore;
using SaveApis.Common.Domain.Tracking.Entities;
using SaveApis.Common.Infrastructure.Persistence.Sql;
using SaveApis.Common.Persistence.Sql.Tracking.Configurations;

namespace SaveApis.Common.Persistence.Sql.Tracking;

public class TrackingDbContext(DbContextOptions options, IMediator mediator) : BaseDbContext(options, mediator)
{
    protected override string Schema => "Tracking";

    public DbSet<TrackingEntryEntity> Entries { get; set; }

    protected override void CreateEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TrackingEntryEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TrackingValueEntityConfiguration());
    }
}
