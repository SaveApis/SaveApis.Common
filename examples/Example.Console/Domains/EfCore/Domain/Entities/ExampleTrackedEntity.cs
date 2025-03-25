using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql.Entities;
using SaveApis.Common.Domains.Tracking.Infrastructure.Attributes;

namespace Example.Console.Domains.EfCore.Domain.Entities;

[EntityTracking]
public partial class ExampleTrackedEntity : IEntity
{
    public Id Id { get; }
    public string Test { get; private set; }
    public int TestInt { get; private set; }

    [AnonymizeTracking]
    public string AnonymizedTest { get; private set; }

    [IgnoreTracking]
    public string IgnoredTest { get; private set; }
}
