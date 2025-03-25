using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Common.Domains.Core.Infrastructure.DI;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;

namespace SaveApis.Common.Domains.Mediator.Application.DI;

public class MediatorModule(IAssemblyHelper assemblyHelper) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assemblyHelper.GetRegisteredAssemblies().ToArray()));

        builder.Populate(collection);
    }
}
