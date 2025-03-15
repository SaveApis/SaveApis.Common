using FluentResults;
using MediatR;

namespace SaveApis.Common.Infrastructure.Mediator;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
