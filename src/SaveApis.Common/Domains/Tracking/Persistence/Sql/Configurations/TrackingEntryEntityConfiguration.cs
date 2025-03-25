using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.Tracking.Domain.Entities;

namespace SaveApis.Common.Domains.Tracking.Persistence.Sql.Configurations;

public class TrackingEntryEntityConfiguration : IEntityTypeConfiguration<TrackingEntryEntity>
{
    public void Configure(EntityTypeBuilder<TrackingEntryEntity> builder)
    {
        builder.ToTable("Entries");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).IsRequired().HasConversion<Id.EfCoreValueConverter>();
        builder.Property(e => e.AffectedEntityId).IsRequired().HasConversion<Id.EfCoreValueConverter>();
        builder.Property(e => e.TrackedAt).IsRequired();
        builder.Property(e => e.TrackingType).IsRequired().HasConversion<string>();

        builder.HasMany(e => e.Values).WithOne(e => e.TrackingEntry).HasForeignKey(v => v.TrackingEntryId);
    }
}
