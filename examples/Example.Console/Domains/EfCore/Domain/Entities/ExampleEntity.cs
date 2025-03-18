using SaveApis.Common.Domain.VOs;
using SaveApis.Common.Infrastructure.Persistence.Sql.Entities;
using SaveApis.Generator.EfCore.Infrastructure.Persistence.Sql.Entities.Attributes;

namespace Example.Console.Domains.EfCore.Domain.Entities;

[Entity]
public partial class ExampleEntity : IEntity
{
    public Id Id { get; }
    public string Name { get; private set; }
}
