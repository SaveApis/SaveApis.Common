using SaveApis.Common.Domain.Tracking.Dtos;
using SaveApis.Common.Domain.VOs;
using SaveApis.Common.Infrastructure.Mediator;

namespace SaveApis.Common.Application.Tracking.Mediator.Queries.GetTrackedEntries;

public record GetTrackedEntriesQuery(Id Id) : IQuery<IReadOnlyCollection<TrackingEntryGetDto>>;
