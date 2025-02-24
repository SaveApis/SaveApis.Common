using Autofac;
using SaveApis.Common.Application.Helper;
using SaveApis.Common.Infrastructure.Builder;
using SaveApis.Common.Infrastructure.DI;
using SaveApis.Common.Infrastructure.Helper;

namespace SaveApis.Common.Application.DI;

public class CoreModule(IAssemblyHelper helper) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(helper).As<IAssemblyHelper>().SingleInstance();
        builder.RegisterType<TypeHelper>().As<ITypeHelper>();

        builder.RegisterAssemblyTypes(helper.GetAssemblies().ToArray())
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBuilder<>)))
            .AsImplementedInterfaces()
            .InstancePerDependency();
    }
}
