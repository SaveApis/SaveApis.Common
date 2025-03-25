using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.Mediator.Infrastructure.Queries;
using SaveApis.Common.Domains.Tracking.Domain.Dtos;

namespace SaveApis.Common.Domains.Tracking.Application.Mediator.Queries.GetTrackedEntries;

public record GetTrackedEntriesQuery(Id Id) : IQuery<IReadOnlyCollection<TrackingEntryGetDto>>;
