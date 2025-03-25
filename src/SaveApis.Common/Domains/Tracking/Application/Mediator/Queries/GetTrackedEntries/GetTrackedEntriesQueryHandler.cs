using FluentResults;
using Microsoft.EntityFrameworkCore;
using SaveApis.Common.Domains.Mediator.Infrastructure.Queries;
using SaveApis.Common.Domains.Tracking.Domain.Dtos;
using SaveApis.Common.Domains.Tracking.Infrastructure.Mapper;
using SaveApis.Common.Domains.Tracking.Persistence.Sql.Factories;

namespace SaveApis.Common.Domains.Tracking.Application.Mediator.Queries.GetTrackedEntries;

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
