using Example.Console.Domains.EfCore.Domain.Entities;
using Example.Console.Domains.EfCore.Persistence.Sql.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SaveApis.Common.Infrastructure.Persistence.Sql;

namespace Example.Console.Domains.EfCore.Persistence.Sql;

public class ExampleDbContext(DbContextOptions options, IMediator mediator) : BaseDbContext(options, mediator)
{
    protected override string Schema => "Example";

    public DbSet<ExampleEntity> Entities { get; set; }
    public DbSet<ExampleTrackedEntity> TrackedEntities { get; set; }

    protected override void CreateEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ExampleEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ExampleTrackedEntityConfiguration());
    }
}
