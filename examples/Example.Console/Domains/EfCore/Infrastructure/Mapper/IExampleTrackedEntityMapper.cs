using Example.Console.Domains.EfCore.Domain.Dtos;
using Example.Console.Domains.EfCore.Domain.Entities;
using SaveApis.Common.Infrastructure.Mapper;

namespace Example.Console.Domains.EfCore.Infrastructure.Mapper;

public interface IExampleTrackedEntityMapper : IMapper
{
    ExampleTrackedEntityDto EntityToDto(ExampleTrackedEntity entity);
}
