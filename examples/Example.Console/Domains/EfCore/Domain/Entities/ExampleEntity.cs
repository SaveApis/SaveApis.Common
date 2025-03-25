using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql.Entities;

namespace Example.Console.Domains.EfCore.Domain.Entities;

public partial class ExampleEntity : IEntity
{
    public Id Id { get; }
    public string Name { get; private set; }
}
