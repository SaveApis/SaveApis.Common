using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Common.Infrastructure.DI;
using SaveApis.Common.Infrastructure.Helper;

namespace SaveApis.Common.Application.DI;

public class MediatorModule(IAssemblyHelper assemblyHelper) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assemblyHelper.GetRegisteredAssemblies().ToArray()));

        builder.Populate(collection);
    }
}
