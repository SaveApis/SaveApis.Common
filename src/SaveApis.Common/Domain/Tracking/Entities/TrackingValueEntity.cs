﻿using SaveApis.Common.Domain.VOs;
using SaveApis.Common.Infrastructure.Persistence.Sql.Entities;
using SaveApis.Generator.EfCore.Infrastructure.Persistence.Sql.Entities.Attributes;

namespace SaveApis.Common.Domain.Tracking.Entities;

[Entity]
public partial class TrackingValueEntity : IEntity
{
    public Id Id { get; }
    public string PropertyName { get; }
    public string? OldValue { get; }
    public string? NewValue { get; }

    public Id TrackingEntryId { get; }
    public virtual TrackingEntryEntity TrackingEntry { get; set; }
}
