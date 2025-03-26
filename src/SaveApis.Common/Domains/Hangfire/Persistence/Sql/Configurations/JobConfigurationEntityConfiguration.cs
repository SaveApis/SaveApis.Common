using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.Hangfire.Domain.Entities;

namespace SaveApis.Common.Domains.Hangfire.Persistence.Sql.Configurations;

public class JobConfigurationEntityConfiguration : IEntityTypeConfiguration<JobConfigurationEntity>
{
    public void Configure(EntityTypeBuilder<JobConfigurationEntity> builder)
    {
        builder.ToTable("JobConfigurations");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id).IsRequired().HasConversion<Id.EfCoreValueConverter>();
        builder.Property(e => e.Namespace).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Value).IsRequired().HasMaxLength(1000);

        builder.HasIndex(x => new { x.Namespace, x.Name }).IsUnique();
    }
}
