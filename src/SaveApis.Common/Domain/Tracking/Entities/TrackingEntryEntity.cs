using SaveApis.Common.Domain.Tracking.Types;
using SaveApis.Common.Domain.VOs;
using SaveApis.Common.Infrastructure.Persistence.Sql.Entities;

namespace SaveApis.Common.Domain.Tracking.Entities;

public partial class TrackingEntryEntity : IEntity
{
    public Id Id { get; }
    public Id AffectedEntityId { get; }
    public DateTime TrackedAt { get; }
    public TrackingType TrackingType { get; }

    public virtual ICollection<TrackingValueEntity> Values { get; set; } = [];

    public TrackingEntryEntity WithValues(IEnumerable<TrackingValueEntity> values)
    {
        Values = values.ToList();

        return this;
    }
}
