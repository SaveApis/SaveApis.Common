using MediatR;
using Microsoft.EntityFrameworkCore;
using SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql;

namespace SaveApis.Common.Domains.Hangfire.Persistence.Sql;

public class HangfireDbContext(DbContextOptions options, IMediator mediator) : BaseDbContext(options, mediator)
{
    protected override string Schema => "Hangfire";

    protected override void CreateEntities(ModelBuilder modelBuilder)
    {
    }
}
