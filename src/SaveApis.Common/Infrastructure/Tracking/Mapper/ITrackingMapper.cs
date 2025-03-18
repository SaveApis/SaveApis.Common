using SaveApis.Common.Domain.Tracking.Dtos;
using SaveApis.Common.Domain.Tracking.Entities;
using SaveApis.Common.Infrastructure.Mapper;

namespace SaveApis.Common.Infrastructure.Tracking.Mapper;

public interface ITrackingMapper : IMapper
{
    TrackingEntryGetDto MapToDto(TrackingEntryEntity entity);
    IReadOnlyCollection<TrackingEntryGetDto> MapToDto(IReadOnlyCollection<TrackingEntryEntity> entities);

    TrackingValueGetDto MapToDto(TrackingValueEntity entity);
    IReadOnlyCollection<TrackingValueGetDto> MapToDto(IReadOnlyCollection<TrackingValueEntity> entities);
}
