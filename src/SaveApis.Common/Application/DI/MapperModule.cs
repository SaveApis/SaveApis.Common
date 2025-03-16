using Autofac;
using SaveApis.Common.Infrastructure.DI;
using SaveApis.Common.Infrastructure.Helper;
using SaveApis.Common.Infrastructure.Mapper;

namespace SaveApis.Common.Application.DI;

public class MapperModule(IAssemblyHelper assemblyHelper) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var assemblies = assemblyHelper.GetRegisteredAssemblies().ToArray();

        builder.RegisterAssemblyTypes(assemblies)
            .Where(it => it.IsAssignableTo<IMapper>())
            .AsImplementedInterfaces();
        builder.RegisterAssemblyOpenGenericTypes(assemblies)
            .Where(it => it.IsAssignableTo(typeof(IMapper)))
            .AsImplementedInterfaces();
    }
}
