using FluentResults;
using SaveApis.Common.Infrastructure.Mediator;

namespace Example.Console.Domains.Mediator.Application.Mediator.Queries.NormalQuery;

public class NormalQueryQueryHandler : IQueryHandler<NormalQueryQuery, string>
{
    public async Task<Result<string>> Handle(NormalQueryQuery request, CancellationToken cancellationToken)
    {
        return "Hello, World!";
    }
}
