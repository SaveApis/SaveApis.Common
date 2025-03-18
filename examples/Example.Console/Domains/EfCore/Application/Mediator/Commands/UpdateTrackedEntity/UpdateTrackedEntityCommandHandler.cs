using Example.Console.Domains.EfCore.Persistence.Sql.Factories;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SaveApis.Common.Domain.VOs;
using SaveApis.Common.Infrastructure.Mediator;

namespace Example.Console.Domains.EfCore.Application.Mediator.Commands.UpdateTrackedEntity;

public class UpdateTrackedEntityCommandHandler(IExampleDbContextFactory factory) : ICommandHandler<UpdateTrackedEntityCommand, Id>
{
    public async Task<Result<Id>> Handle(UpdateTrackedEntityCommand request, CancellationToken cancellationToken)
    {
        await using var context = factory.Create();

        var entity = await context.TrackedEntities.SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken).ConfigureAwait(false);

        if (entity is null)
        {
            return Result.Fail($"Entity with id {request.Id} not found");
        }

        entity.UpdateTest($"Test-{Guid.NewGuid()}");
        entity.UpdateTestInt(Random.Shared.Next());
        entity.UpdateAnonymizedTest($"Anonymized-{Guid.NewGuid()}");
        entity.UpdateIgnoredTest($"Ignored-{Guid.NewGuid()}");
        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return entity.Id;
    }
}
