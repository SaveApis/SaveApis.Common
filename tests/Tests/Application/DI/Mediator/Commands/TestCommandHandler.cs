using FluentResults;
using SaveApis.Common.Infrastructure.Mediator;

namespace Tests.Application.DI.Mediator.Commands;

public class TestCommandHandler : IQueryHandler<TestCommand, string>
{
    public Task<Result<string>> Handle(TestCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Ok("Test"));
    }
}
