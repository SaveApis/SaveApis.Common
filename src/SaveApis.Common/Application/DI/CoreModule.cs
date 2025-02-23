using Autofac;
using SaveApis.Common.Application.Helper;
using SaveApis.Common.Infrastructure.DI;
using SaveApis.Common.Infrastructure.Helper;

namespace SaveApis.Common.Application.DI;

public class CoreModule(IAssemblyHelper helper) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(helper).As<IAssemblyHelper>().SingleInstance();
        builder.RegisterType<TypeHelper>().As<ITypeHelper>();
    }
}
