using FluentResults;
using MediatR;

namespace SaveApis.Common.Infrastructure.Mediator;

public interface ICommand<TResult> : IRequest<Result<TResult>>;
