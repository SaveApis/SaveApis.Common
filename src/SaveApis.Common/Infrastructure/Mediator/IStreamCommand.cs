using FluentResults;
using MediatR;

namespace SaveApis.Common.Infrastructure.Mediator;

public interface IStreamCommand : IStreamRequest<Result>;

public interface IStreamCommand<TResult> : IStreamRequest<Result<TResult>>;
