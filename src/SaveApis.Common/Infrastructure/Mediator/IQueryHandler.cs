using FluentResults;
using MediatR;

namespace SaveApis.Common.Infrastructure.Mediator;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, Result<TResult>> where TQuery : IQuery<TResult>;
