using Example.Console.Domains.EfCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveApis.Common.Domain.VOs;

namespace Example.Console.Domains.EfCore.Persistence.Sql.Configurations;

public class ExampleEntityConfiguration : IEntityTypeConfiguration<ExampleEntity>
{
    public void Configure(EntityTypeBuilder<ExampleEntity> builder)
    {
        builder.ToTable("Entities");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasConversion<Id.EfCoreValueConverter>();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
    }
}
