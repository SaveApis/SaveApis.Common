using Example.Console.Domains.Mapper.Domain.Dtos;
using Example.Console.Domains.Mapper.Domain.Models;
using SaveApis.Common.Domains.Mapper.Infrastructure;

namespace Example.Console.Domains.Mapper.Infrastructure.Mapper;

public interface IExampleMapper : IMapper
{
    ExampleDto EntityToDto(ExampleModel model);
}
