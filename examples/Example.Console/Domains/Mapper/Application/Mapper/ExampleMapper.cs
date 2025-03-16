using Example.Console.Domains.Mapper.Domain.Dtos;
using Example.Console.Domains.Mapper.Domain.Models;
using Example.Console.Domains.Mapper.Infrastructure.Mapper;

namespace Example.Console.Domains.Mapper.Application.Mapper;

public class ExampleMapper : IExampleMapper
{
    public ExampleDto EntityToDto(ExampleModel model)
    {
        return new ExampleDto
        {
            Id = model.Id,
            Name = model.Name,
        };
    }
}
