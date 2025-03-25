using FluentResults;
using MediatR;

namespace SaveApis.Common.Domains.Mediator.Infrastructure.Queries;

public interface IStreamQueryHandler<in TRequest, TResponse> : IStreamRequestHandler<TRequest, Result<TResponse>> where TRequest : IStreamQuery<TResponse>;
