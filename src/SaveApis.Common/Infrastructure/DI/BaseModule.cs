using Autofac;

namespace SaveApis.Common.Infrastructure.DI;

public abstract class BaseModule : Module
{
    protected override abstract void Load(ContainerBuilder builder);
}
