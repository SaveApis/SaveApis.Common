using SaveApis.Common.Domain.VOs;

namespace Example.Console.Domains.EfCore.Domain.Dtos;

public class ExampleTrackedEntityDto
{
    public required Id Id { get; init; }
    public required string Test { get; init; }
    public required int TestInt { get; init; }
    public required string AnonymizedTest { get; init; }
    public required string IgnoredTest { get; init; }
}
