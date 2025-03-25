using FluentResults;
using MediatR;

namespace SaveApis.Common.Domains.Mediator.Infrastructure.Queries;

public interface IQuery<TResult> : IRequest<Result<TResult>>;
