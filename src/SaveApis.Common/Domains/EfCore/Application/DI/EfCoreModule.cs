using Autofac;
using Microsoft.EntityFrameworkCore.Design;
using SaveApis.Common.Domains.Core.Infrastructure.DI;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;
using SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql;

namespace SaveApis.Common.Domains.EfCore.Application.DI;

public class EfCoreModule(IAssemblyHelper assemblyHelper) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(assemblyHelper.GetRegisteredAssemblies().ToArray())
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDesignTimeDbContextFactory<>)))
            .AsImplementedInterfaces()
            .As<IDesignTimeDbContextFactory<BaseDbContext>>();
    }
}
