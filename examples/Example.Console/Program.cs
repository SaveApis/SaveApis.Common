// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaveApis.Common.Domains.Core.Application.DI;
using SaveApis.Common.Domains.Core.Application.Helper;
using SaveApis.Common.Domains.Core.Infrastructure.Extensions;
using SaveApis.Common.Domains.EfCore.Application.DI;
using SaveApis.Common.Domains.Hangfire.Application.DI;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Extensions;
using SaveApis.Common.Domains.Logging.Application.DI;
using SaveApis.Common.Domains.Mapper.Application.DI;
using SaveApis.Common.Domains.Mediator.Application.DI;

var builder = Host.CreateApplicationBuilder(args);

var hangfireType = builder.Configuration.GetHangfireType();
var assemblyHelper = new AssemblyHelper(Assembly.GetExecutingAssembly());
builder.ConfigureContainer(new AutofacServiceProviderFactory(), containerBuilder =>
{
    containerBuilder.WithModule<CoreModule>(null, assemblyHelper);
    containerBuilder.WithModule<MediatorModule>(null, assemblyHelper);
    containerBuilder.WithModule<HangfireModule>(null, assemblyHelper, builder.Configuration, hangfireType);
    containerBuilder.WithModule<EfCoreModule>(null, assemblyHelper);
    containerBuilder.WithModule<MapperModule>(null, assemblyHelper);

    containerBuilder.WithModule<SerilogModule>(null, builder.Configuration);
});

var app = builder.Build();

await app.RunAsync().ConfigureAwait(false);
