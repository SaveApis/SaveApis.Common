using FluentResults;
using SaveApis.Common.Infrastructure.Mediator;

namespace Example.Console.Domains.Mediator.Application.Mediator.Queries.StreamQuery;

public class StreamQueryQueryHandler : IStreamQueryHandler<StreamQueryQuery, string>
{
    public async IAsyncEnumerable<Result<string>> Handle(StreamQueryQuery request, CancellationToken cancellationToken)
    {
        await Task.Delay(1000, cancellationToken).ConfigureAwait(false);

        yield return "Hello, World 1!";

        await Task.Delay(1000, cancellationToken).ConfigureAwait(false);

        yield return "Hello, World 2!";

        await Task.Delay(1000, cancellationToken).ConfigureAwait(false);

        yield return "Hello, World 3!";

        await Task.Delay(1000, cancellationToken).ConfigureAwait(false);

        yield return "Hello, World 4!";
    }
}
