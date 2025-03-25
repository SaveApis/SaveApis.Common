using FluentResults;
using MediatR;

namespace SaveApis.Common.Domains.Mediator.Infrastructure.Commands;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
