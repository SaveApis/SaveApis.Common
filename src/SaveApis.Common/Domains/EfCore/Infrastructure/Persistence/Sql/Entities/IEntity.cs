using SaveApis.Common.Domains.Core.Domain.VOs;

namespace SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql.Entities;

public interface IEntity
{
    Id Id { get; }
}
