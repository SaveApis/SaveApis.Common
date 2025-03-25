using Autofac;
using SaveApis.Common.Domains.Core.Infrastructure.DI;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;
using SaveApis.Common.Domains.Mapper.Infrastructure;

namespace SaveApis.Common.Domains.Mapper.Application.DI;

public class MapperModule(IAssemblyHelper assemblyHelper) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var assemblies = assemblyHelper.GetRegisteredAssemblies().ToArray();

        builder.RegisterAssemblyTypes(assemblies)
            .Where(it => it.IsAssignableTo<IMapper>())
            .AsImplementedInterfaces();
        builder.RegisterAssemblyOpenGenericTypes(assemblies)
            .Where(it => it.IsAssignableTo<IMapper>())
            .AsImplementedInterfaces();
    }
}
