using Autofac;
using Autofac.Extensions.DependencyInjection;
using Correlate.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Common.Infrastructure.DI;

namespace SaveApis.Common.Application.DI;

public class CorrelateModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddCorrelate();
        collection.ConfigureHttpClientDefaults(clientBuilder => clientBuilder.CorrelateRequests());

        builder.Populate(collection);
    }
}
