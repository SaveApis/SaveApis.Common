using System.Reflection;
using Autofac;
using Riok.Mapperly.Abstractions;
using SaveApis.Common.Infrastructure.DI;
using SaveApis.Common.Infrastructure.Helper;

namespace SaveApis.Common.Application.DI;

public class MapperModule(IAssemblyHelper helper) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(helper.GetAssemblies().ToArray())
            .Where(t => t.GetCustomAttribute<MapperAttribute>() is not null)
            .AsImplementedInterfaces()
            .AsSelf()
            .InstancePerDependency();
    }
}
