using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.Tracking.Domain.Entities;

namespace SaveApis.Common.Domains.Tracking.Persistence.Sql.Configurations;

public class TrackingValueEntityConfiguration : IEntityTypeConfiguration<TrackingValueEntity>
{
    public void Configure(EntityTypeBuilder<TrackingValueEntity> builder)
    {
        builder.ToTable("EntryValues");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasConversion<Id.EfCoreValueConverter>();
        builder.Property(e => e.PropertyName).IsRequired();
        builder.Property(e => e.OldValue).IsRequired(false);
        builder.Property(e => e.NewValue).IsRequired(false);

        builder.Property(e => e.TrackingEntryId).IsRequired().HasConversion<Id.EfCoreValueConverter>();

        builder.HasOne(e => e.TrackingEntry).WithMany(e => e.Values).HasForeignKey(e => e.TrackingEntryId);
    }
}
