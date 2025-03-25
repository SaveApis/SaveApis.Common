using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.Tracking.Domain.Types;

namespace SaveApis.Common.Domains.Tracking.Domain.Dtos;

public class TrackingEntryGetDto
{
    public required Id Id { get; init; }
    public required DateTime TrackedAt { get; init; }
    public required TrackingType TrackingType { get; init; }

    public required IReadOnlyCollection<TrackingValueGetDto> Values { get; init; }
}
