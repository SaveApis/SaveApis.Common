using FluentResults;
using SaveApis.Common.Domains.Mediator.Infrastructure.Queries;

namespace Example.Console.Domains.Mediator.Application.Mediator.Queries.NormalQuery;

public class NormalQueryQueryHandler : IQueryHandler<NormalQueryQuery, string>
{
    public async Task<Result<string>> Handle(NormalQueryQuery request, CancellationToken cancellationToken)
    {
        return "Hello, World!";
    }
}
