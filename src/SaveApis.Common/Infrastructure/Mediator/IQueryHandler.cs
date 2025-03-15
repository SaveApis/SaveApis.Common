using FluentResults;
using MediatR;

namespace SaveApis.Common.Infrastructure.Mediator;

public interface IQueryHandler<TRequest, TResult> : IRequestHandler<TRequest, Result<TResult>> where TRequest : IQuery<TResult>;
