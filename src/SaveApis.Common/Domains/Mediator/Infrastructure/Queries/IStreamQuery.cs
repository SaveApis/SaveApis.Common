using FluentResults;
using MediatR;

namespace SaveApis.Common.Domains.Mediator.Infrastructure.Queries;

public interface IStreamQuery<TResult> : IStreamRequest<Result<TResult>>;
