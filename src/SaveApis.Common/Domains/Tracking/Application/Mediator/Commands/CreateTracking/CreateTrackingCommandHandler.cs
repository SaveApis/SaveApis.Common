using FluentResults;
using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.Mediator.Infrastructure.Commands;
using SaveApis.Common.Domains.Tracking.Domain.Entities;
using SaveApis.Common.Domains.Tracking.Persistence.Sql.Factories;

namespace SaveApis.Common.Domains.Tracking.Application.Mediator.Commands.CreateTracking;

public class CreateTrackingCommandHandler(ITrackingDbContextFactory factory) : ICommandHandler<CreateTrackingCommand, Id>
{
    public async Task<Result<Id>> Handle(CreateTrackingCommand request, CancellationToken cancellationToken)
    {
        await using var context = factory.Create();

        var entry = TrackingEntryEntity.Create(Id.From(Guid.NewGuid()), request.AffectedEntityId, DateTime.UtcNow, request.TrackingType);

        var values = new List<TrackingValueEntity>();
        foreach (var (propertyName, oldValue, newValue) in request.Values)
        {
            values.Add(TrackingValueEntity.Create(Id.From(Guid.NewGuid()), propertyName, oldValue, newValue, entry.Id));
        }

        entry.WithValues(values);

        context.Entries.Add(entry);
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return entry.Id;
    }
}
