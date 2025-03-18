using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SaveApis.Common.Application.Tracking.Mediator.Commands.CreateTracking;
using SaveApis.Common.Domain.Tracking.Types;
using SaveApis.Common.Infrastructure.Persistence.Sql.Entities;
using SaveApis.Generator.EfCore.Infrastructure.Persistence.Sql.Entities.Attributes;

namespace SaveApis.Common.Infrastructure.Persistence.Sql;

public abstract class BaseDbContext(DbContextOptions options, IMediator mediator) : DbContext(options)
{
    protected abstract string Schema { get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema);
        CreateEntities(modelBuilder);
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not IEntity idEntity)
            {
                continue;
            }

            var attributes = CheckTrackingAttribute(entry);
            if (!attributes)
            {
                continue;
            }

            var command = entry.State switch
            {
                EntityState.Added => new CreateTrackingCommand(idEntity.Id, TrackingType.Create),
                EntityState.Modified => new CreateTrackingCommand(idEntity.Id, TrackingType.Update),
                _ => null,
            };

            if (entry.State == EntityState.Modified)
            {
                foreach (var property in entry.Properties)
                {
                    HandleProperty(property, ref command);
                }
            }

            if (command is not null)
            {
                await mediator.Send(command, cancellationToken).ConfigureAwait(false);
            }
        }

        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private static void HandleProperty(PropertyEntry property, ref CreateTrackingCommand? command)
    {
        var isIgnored = CheckIgnoredAttribute(property.Metadata.PropertyInfo);
        if (isIgnored)
        {
            return;
        }

        if (!property.IsModified)
        {
            return;
        }

        var isAnonymized = CheckAnonymizedAttribute(property.Metadata.PropertyInfo);
        var oldValue = property.OriginalValue?.ToString() is null
            ? null
            : isAnonymized
                ? "***"
                : property.OriginalValue?.ToString();
        var newValue = property.CurrentValue?.ToString() is null
            ? null
            : isAnonymized
                ? "***"
                : property.CurrentValue?.ToString();

        command?.AddValue(property.Metadata.Name, oldValue, newValue);
    }

    private static bool CheckTrackingAttribute(EntityEntry entry)
    {
        var attributes = entry.Entity.GetType().GetCustomAttributes().Select(it => it.GetType().Name).Distinct();

        return attributes.Contains(nameof(TrackedEntityAttribute));
    }
    private static bool CheckIgnoredAttribute(PropertyInfo? metadataPropertyInfo)
    {
        if (metadataPropertyInfo is null)
        {
            return false;
        }

        var attributes = metadataPropertyInfo.GetCustomAttributes().Select(it => it.GetType().Name).Distinct();

        return attributes.Contains(nameof(IgnoreTrackingAttribute));
    }
    private static bool CheckAnonymizedAttribute(PropertyInfo? metadataPropertyInfo)
    {
        if (metadataPropertyInfo is null)
        {
            return false;
        }

        var attributes = metadataPropertyInfo.GetCustomAttributes().Select(it => it.GetType().Name).Distinct();

        return attributes.Contains(nameof(AnonymizeTrackingAttribute));
    }

    protected abstract void CreateEntities(ModelBuilder modelBuilder);
}
