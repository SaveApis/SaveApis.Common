using FluentResults;
using MediatR;

namespace SaveApis.Common.Domains.Mediator.Infrastructure.Commands;

public interface IStreamCommand : IStreamRequest<Result>;

public interface IStreamCommand<TResult> : IStreamRequest<Result<TResult>>;
