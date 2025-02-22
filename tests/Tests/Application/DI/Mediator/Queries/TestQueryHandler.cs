using FluentResults;
using SaveApis.Common.Infrastructure.Mediator;

namespace Tests.Application.DI.Mediator.Queries;

public class TestQueryHandler : IQueryHandler<TestQuery, string>
{
    public Task<Result<string>> Handle(TestQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result.Ok("Test"));
    }
}
