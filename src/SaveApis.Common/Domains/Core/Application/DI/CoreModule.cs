using Autofac;
using SaveApis.Common.Domains.Core.Application.Helper;
using SaveApis.Common.Domains.Core.Infrastructure.DI;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;

namespace SaveApis.Common.Domains.Core.Application.DI;

public class CoreModule(IAssemblyHelper helper) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(helper).As<IAssemblyHelper>().SingleInstance();
        builder.RegisterType<TypeHelper>().As<ITypeHelper>();
    }
}
