using SaveApis.Common.Domain.Tracking.Types;
using SaveApis.Common.Domain.VOs;

namespace SaveApis.Common.Domain.Tracking.Dtos;

public class TrackingEntryGetDto
{
    public required Id Id { get; init; }
    public required DateTime TrackedAt { get; init; }
    public required TrackingType TrackingType { get; init; }

    public required IReadOnlyCollection<TrackingValueGetDto> Values { get; init; }
}
