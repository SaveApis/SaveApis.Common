using SaveApis.Common.Domains.Tracking.Domain.Dtos;
using SaveApis.Common.Domains.Tracking.Domain.Entities;
using SaveApis.Common.Domains.Tracking.Infrastructure.Mapper;

namespace SaveApis.Common.Domains.Tracking.Application.Mapper;

public class TrackingMapper : ITrackingMapper
{
    public TrackingEntryGetDto MapToDto(TrackingEntryEntity entity)
    {
        return new TrackingEntryGetDto
        {
            Id = entity.Id,
            TrackedAt = entity.TrackedAt,
            TrackingType = entity.TrackingType,
            Values = entity.Values.Select(MapToDto).ToList(),
        };
    }

    public IReadOnlyCollection<TrackingEntryGetDto> MapToDto(IReadOnlyCollection<TrackingEntryEntity> entities)
    {
        return [.. entities.Select(MapToDto)];
    }

    public TrackingValueGetDto MapToDto(TrackingValueEntity entity)
    {
        return new TrackingValueGetDto
        {
            Id = entity.Id,
            PropertyName = entity.PropertyName,
            OldValue = entity.OldValue,
            NewValue = entity.NewValue,
        };
    }

    public IReadOnlyCollection<TrackingValueGetDto> MapToDto(IReadOnlyCollection<TrackingValueEntity> entities)
    {
        return [.. entities.Select(MapToDto)];
    }
}
