using SaveApis.Common.Domain.VOs;

namespace SaveApis.Common.Infrastructure.Persistence.Sql.Entities;

public interface IEntity
{
    Id Id { get; }
}
