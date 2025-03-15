using FluentResults;
using MediatR;

namespace SaveApis.Common.Infrastructure.Mediator;

public interface IQuery<TResult> : IRequest<Result<TResult>>;
