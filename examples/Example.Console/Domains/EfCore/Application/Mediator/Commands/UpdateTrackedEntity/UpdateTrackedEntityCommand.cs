using SaveApis.Common.Domains.Core.Domain.VOs;
using SaveApis.Common.Domains.Mediator.Infrastructure.Commands;

namespace Example.Console.Domains.EfCore.Application.Mediator.Commands.UpdateTrackedEntity;

public record UpdateTrackedEntityCommand(Id Id) : ICommand<Id>;
