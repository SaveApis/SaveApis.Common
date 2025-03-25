using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.Mediator.Infrastructure.Commands;

namespace Example.Console.Domains.EfCore.Application.Mediator.Commands.CreateTrackedEntity;

public record CreateTrackedEntityCommand : ICommand<Id>;
