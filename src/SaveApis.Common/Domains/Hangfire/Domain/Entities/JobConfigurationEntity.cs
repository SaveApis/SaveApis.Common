using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql.Entities;

namespace SaveApis.Common.Domains.Hangfire.Domain.Entities;

public partial class JobConfigurationEntity : IEntity
{
    public Id Id { get; }

    public string Namespace { get; }
    public string Name { get; }
    public string Value { get; }
}
