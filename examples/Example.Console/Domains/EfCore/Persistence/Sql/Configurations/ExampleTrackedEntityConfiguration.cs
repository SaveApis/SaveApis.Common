using Example.Console.Domains.EfCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveApis.Common.Domain.VOs;

namespace Example.Console.Domains.EfCore.Persistence.Sql.Configurations;

public class ExampleTrackedEntityConfiguration : IEntityTypeConfiguration<ExampleTrackedEntity>
{
    public void Configure(EntityTypeBuilder<ExampleTrackedEntity> builder)
    {
        builder.ToTable("TrackedEntities");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasConversion<Id.EfCoreValueConverter>();
        builder.Property(e => e.Test).IsRequired().HasMaxLength(100);
        builder.Property(e => e.TestInt).IsRequired();
        builder.Property(e => e.AnonymizedTest).IsRequired().HasMaxLength(100);
        builder.Property(e => e.IgnoredTest).IsRequired().HasMaxLength(100);
    }
}
