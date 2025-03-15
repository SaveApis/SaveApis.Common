using FluentResults;
using MediatR;

namespace SaveApis.Common.Infrastructure.Mediator;

public interface IStreamQuery<TResult> : IStreamRequest<Result<TResult>>;
