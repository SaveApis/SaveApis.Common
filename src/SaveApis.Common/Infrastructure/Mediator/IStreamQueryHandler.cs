using FluentResults;
using MediatR;

namespace SaveApis.Common.Infrastructure.Mediator;

public interface IStreamQueryHandler<TRequest, TResponse> : IStreamRequestHandler<TRequest, Result<TResponse>> where TRequest : IStreamQuery<TResponse>;
