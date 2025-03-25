using SaveApis.Common.Domains.Mapper.Infrastructure;
using SaveApis.Common.Domains.Tracking.Domain.Dtos;
using SaveApis.Common.Domains.Tracking.Domain.Entities;

namespace SaveApis.Common.Domains.Tracking.Infrastructure.Mapper;

public interface ITrackingMapper : IMapper
{
    TrackingEntryGetDto MapToDto(TrackingEntryEntity entity);
    IReadOnlyCollection<TrackingEntryGetDto> MapToDto(IReadOnlyCollection<TrackingEntryEntity> entities);

    TrackingValueGetDto MapToDto(TrackingValueEntity entity);
    IReadOnlyCollection<TrackingValueGetDto> MapToDto(IReadOnlyCollection<TrackingValueEntity> entities);
}
