using SaveApis.Common.Domain.VOs;
using SaveApis.Common.Infrastructure.Mediator;

namespace Example.Console.Domains.EfCore.Application.Mediator.Commands.UpdateTrackedEntity;

public record UpdateTrackedEntityCommand(Id Id) : ICommand<Id>;
