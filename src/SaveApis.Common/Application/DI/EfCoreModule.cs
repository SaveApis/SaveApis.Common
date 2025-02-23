using Autofac;
using Microsoft.EntityFrameworkCore.Design;
using SaveApis.Common.Infrastructure.DI;
using SaveApis.Common.Infrastructure.Helper;
using SaveApis.Common.Infrastructure.Persistence.Sql;

namespace SaveApis.Common.Application.DI;

public class EfCoreModule(IAssemblyHelper helper) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(helper.GetAssemblies().ToArray())
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDesignTimeDbContextFactory<>)))
            .AsImplementedInterfaces()
            .As<IDesignTimeDbContextFactory<BaseDbContext>>();
    }
}
