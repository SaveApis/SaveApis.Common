using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql.Entities;

namespace SaveApis.Common.Domains.Hangfire.Domain.Entities;

public partial class RecurringEventEntity : IEntity
{
    public Id Id { get; }
    public string Key { get; }
    public string Cron { get; private set; }
}
