using FluentResults;
using MediatR;

namespace SaveApis.Common.Domains.Mediator.Infrastructure.Commands;

public interface IStreamCommandHandler<TRequest> : IStreamRequestHandler<TRequest, Result> where TRequest : IStreamCommand;

public interface IStreamCommandHandler<TRequest, TResult> : IStreamRequestHandler<TRequest, Result<TResult>> where TRequest : IStreamCommand<TResult>;
