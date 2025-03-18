using Example.Console.Domains.EfCore.Domain.Entities;
using Example.Console.Domains.EfCore.Persistence.Sql.Factories;
using FluentResults;
using SaveApis.Common.Domain.VOs;
using SaveApis.Common.Infrastructure.Mediator;

namespace Example.Console.Domains.EfCore.Application.Mediator.Commands.CreateTrackedEntity;

public class CreateTrackedEntityCommandHandler(IExampleDbContextFactory factory) : ICommandHandler<CreateTrackedEntityCommand, Id>
{
    public async Task<Result<Id>> Handle(CreateTrackedEntityCommand request, CancellationToken cancellationToken)
    {
        await using var context = factory.Create();

        var entity = ExampleTrackedEntity.Create(Id.From(Guid.NewGuid()), $"Test-{Guid.NewGuid()}", Random.Shared.Next(), $"Anonymized-{Guid.NewGuid()}",
            $"Ignored-{Guid.NewGuid()}");

        context.TrackedEntities.Add(entity);

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return entity.Id;
    }
}
