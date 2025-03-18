using SaveApis.Common.Domain.VOs;
using SaveApis.Common.Infrastructure.Persistence.Sql.Entities;
using SaveApis.Generator.EfCore.Infrastructure.Persistence.Sql.Entities.Attributes;

namespace Example.Console.Domains.EfCore.Domain.Entities;

[Entity]
[TrackedEntity]
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
