using SaveApis.Common.Domain.VOs;

namespace SaveApis.Common.Domain.Tracking.Dtos;

public class TrackingValueGetDto
{
    public required Id Id { get; init; }
    public required string PropertyName { get; init; }
    public required string? OldValue { get; init; }
    public required string? NewValue { get; init; }
}
