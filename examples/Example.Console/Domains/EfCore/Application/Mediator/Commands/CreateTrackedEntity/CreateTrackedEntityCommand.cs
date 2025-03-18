using SaveApis.Common.Domain.VOs;
using SaveApis.Common.Infrastructure.Mediator;

namespace Example.Console.Domains.EfCore.Application.Mediator.Commands.CreateTrackedEntity;

public record CreateTrackedEntityCommand : ICommand<Id>;
