using FluentResults;
using Microsoft.EntityFrameworkCore;
using SaveApis.Common.Domain.Tracking.Dtos;
using SaveApis.Common.Infrastructure.Mediator;
using SaveApis.Common.Infrastructure.Tracking.Mapper;
using SaveApis.Common.Persistence.Sql.Tracking.Factories;

namespace SaveApis.Common.Application.Tracking.Mediator.Queries.GetTrackedEntries;

public class GetTrackedEntriesQueryHandler(ITrackingDbContextFactory factory, ITrackingMapper mapper)
    : IQueryHandler<GetTrackedEntriesQuery, IReadOnlyCollection<TrackingEntryGetDto>>
{
    public async Task<Result<IReadOnlyCollection<TrackingEntryGetDto>>> Handle(GetTrackedEntriesQuery request, CancellationToken cancellationToken)
    {
        await using var context = factory.Create();

        var entries = await context.Entries
            .Where(e => e.AffectedEntityId == request.Id)
            .Include(entity => entity.Values)
            .OrderBy(e => e.TrackedAt)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return mapper.MapToDto(entries).ToList();
    }
}
