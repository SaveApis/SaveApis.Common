using SaveApis.Common.Domain.VOs;
using SaveApis.Common.Infrastructure.Persistence.Sql.Entities;

namespace Example.Console.Domains.EfCore.Domain.Entities;

public partial class ExampleEntity : IEntity
{
    public Id Id { get; }
    public string Name { get; private set; }
}
