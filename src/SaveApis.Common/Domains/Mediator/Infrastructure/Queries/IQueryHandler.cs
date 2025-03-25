using FluentResults;
using MediatR;

namespace SaveApis.Common.Domains.Mediator.Infrastructure.Queries;

public interface IQueryHandler<in TRequest, TResult> : IRequestHandler<TRequest, Result<TResult>> where TRequest : IQuery<TResult>;
