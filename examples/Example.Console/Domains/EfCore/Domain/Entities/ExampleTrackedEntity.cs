using SaveApis.Common.Domain.VOs;
using SaveApis.Generator.EfCore.Infrastructure.Persistence.Sql.Entities;

namespace Example.Console.Domains.EfCore.Domain.Entities;

public partial class ExampleTrackedEntity : ITrackedEntity<Id>
{
    public Id Id { get; }
    public string Name { get; private set; }
}
