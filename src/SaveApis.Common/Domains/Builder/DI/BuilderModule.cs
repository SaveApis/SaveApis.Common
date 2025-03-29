using Autofac;
using SaveApis.Common.Domains.Builder.Infrastructure;
using SaveApis.Common.Domains.Core.Infrastructure.DI;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;

namespace SaveApis.Common.Domains.Builder.DI;

public class BuilderModule(IAssemblyHelper assemblyHelper) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var assembly = assemblyHelper.GetRegisteredAssemblies();

        builder.RegisterAssemblyTypes(assembly.ToArray())
            .Where(t => t.GetInterfaces().Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IBuilder<>)))
            .AsImplementedInterfaces();
        builder.RegisterAssemblyOpenGenericTypes(assembly.ToArray())
            .Where(t => t.GetInterfaces().Any(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IBuilder<>)))
            .AsImplementedInterfaces();
    }
}
