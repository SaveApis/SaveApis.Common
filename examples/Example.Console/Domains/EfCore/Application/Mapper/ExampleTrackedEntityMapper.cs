using Example.Console.Domains.EfCore.Domain.Dtos;
using Example.Console.Domains.EfCore.Domain.Entities;
using Example.Console.Domains.EfCore.Infrastructure.Mapper;

namespace Example.Console.Domains.EfCore.Application.Mapper;

public class ExampleTrackedEntityMapper : IExampleTrackedEntityMapper
{
    public ExampleTrackedEntityDto EntityToDto(ExampleTrackedEntity entity)
    {
        return new ExampleTrackedEntityDto
        {
            Id = entity.Id,
            Test = entity.Test,
            TestInt = entity.TestInt,
            AnonymizedTest = entity.AnonymizedTest,
            IgnoredTest = entity.IgnoredTest,
        };
    }
}
