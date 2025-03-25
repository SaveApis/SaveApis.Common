using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql.Entities;
using SaveApis.Common.Domains.Tracking.Domain.Types;

namespace SaveApis.Common.Domains.Tracking.Domain.Entities;

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
