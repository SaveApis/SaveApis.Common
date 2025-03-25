using MediatR;
using Microsoft.EntityFrameworkCore;
using SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql;
using SaveApis.Common.Domains.Tracking.Domain.Entities;
using SaveApis.Common.Domains.Tracking.Persistence.Sql.Configurations;

namespace SaveApis.Common.Domains.Tracking.Persistence.Sql;

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
