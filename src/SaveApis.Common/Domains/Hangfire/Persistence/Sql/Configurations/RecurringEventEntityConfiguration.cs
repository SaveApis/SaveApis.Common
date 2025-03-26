using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.Hangfire.Domain.Entities;

namespace SaveApis.Common.Domains.Hangfire.Persistence.Sql.Configurations;

public class RecurringEventEntityConfiguration : IEntityTypeConfiguration<RecurringEventEntity>
{
    public void Configure(EntityTypeBuilder<RecurringEventEntity> builder)
    {
        builder.ToTable("RecurringEvents");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id).IsRequired().HasConversion<Id.EfCoreValueConverter>();
        builder.Property(e => e.Key).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Cron).IsRequired().HasMaxLength(100);

        builder.HasIndex(e => e.Key).IsUnique();
    }
}
