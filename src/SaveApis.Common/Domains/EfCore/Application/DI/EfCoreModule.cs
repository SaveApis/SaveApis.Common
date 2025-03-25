using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SaveApis.Common.Domains.Core.Infrastructure.DI;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;
using SaveApis.Common.Domains.EfCore.Infrastructure.Persistence.Sql;
using Serilog;

namespace SaveApis.Common.Domains.EfCore.Application.DI;

public class EfCoreModule(IAssemblyHelper assemblyHelper) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(assemblyHelper.GetRegisteredAssemblies().ToArray())
            .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDesignTimeDbContextFactory<>)))
            .AsImplementedInterfaces()
            .As<IDesignTimeDbContextFactory<BaseDbContext>>();

        builder.RegisterBuildCallback(scope =>
        {
            var logger = scope.Resolve<ILogger>();
            var factories = scope.Resolve<IEnumerable<IDesignTimeDbContextFactory<BaseDbContext>>>().ToList();
            if (factories.Count == 0)
            {
                logger.Information("No factories found.");

                return;
            }

            foreach (var factory in factories)
            {
                using var context = factory.CreateDbContext([]);

                logger.Information("Applying migrations for {context}", context.GetType().Name);
                context.Database.Migrate();
                logger.Information("Migrations applied for {context}", context.GetType().Name);
            }
        });
    }
}
