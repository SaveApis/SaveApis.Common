using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using SaveApis.Common.Infrastructure.DI;
using SaveApis.Common.Infrastructure.Helper;
using SaveApis.Common.Infrastructure.Persistence.Sql;

namespace SaveApis.Common.Application.DI;

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
            var logger = scope.Resolve<ILogger<EfCoreModule>>();
            var factories = scope.Resolve<IEnumerable<IDesignTimeDbContextFactory<BaseDbContext>>>().ToList();
            if (factories.Count == 0)
            {
                logger.LogInformation("No factories found.");

                return;
            }

            foreach (var factory in factories)
            {
                using var context = factory.CreateDbContext([]);

                logger.LogInformation("Applying migrations for {context}", context.GetType().Name);
                context.Database.Migrate();
                logger.LogInformation("Migrations applied for {context}", context.GetType().Name);
            }
        });
    }
}
