using Microsoft.EntityFrameworkCore;

namespace SaveApis.Common.Infrastructure.Persistence.Sql;

public abstract class BaseDbContext(DbContextOptions options) : DbContext(options)
{
    protected abstract string Schema { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema);
        RegisterEntities(modelBuilder);
    }

    protected abstract void RegisterEntities(ModelBuilder modelBuilder);
}
