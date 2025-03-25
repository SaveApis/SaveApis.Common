using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql.Entities;
using SaveApis.Common.Domains.Tracking.Application.Mediator.Commands.CreateTracking;
using SaveApis.Common.Domains.Tracking.Domain.Types;
using SaveApis.Common.Domains.Tracking.Infrastructure.Attributes;

namespace SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql;

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
            var entity = entry.Entity;
            if (entity is not IEntity idEntity)
            {
                continue;
            }

            if (!entity.GetType().GetCustomAttributes<EntityTrackingAttribute>().Any())
            {
                continue;
            }

            await HandleEntityAsync(entry, idEntity, cancellationToken).ConfigureAwait(false);
        }

        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task HandleEntityAsync(EntityEntry entry, IEntity idEntity, CancellationToken cancellationToken)
    {
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

    private static void HandleProperty(PropertyEntry property, ref CreateTrackingCommand? command)
    {
        if (!property.IsModified)
        {
            return;
        }

        var isIgnored = property.Metadata.PropertyInfo?.GetCustomAttributes<IgnoreTrackingAttribute>().Any() ?? false;
        if (isIgnored)
        {
            return;
        }

        var isAnonymized = property.Metadata.PropertyInfo?.GetCustomAttributes<AnonymizeTrackingAttribute>().Any() ?? false;
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

    protected abstract void CreateEntities(ModelBuilder modelBuilder);
}
