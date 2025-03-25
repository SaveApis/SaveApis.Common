using Autofac;

namespace SaveApis.Common.Domains.Core.Infrastructure.DI;

public abstract class BaseModule : Module
{
    protected abstract override void Load(ContainerBuilder builder);
}
