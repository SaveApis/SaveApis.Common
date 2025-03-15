// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Example.Console.Domains.Mediator.Application.Mediator.Queries.NormalQuery;
using Example.Console.Domains.Mediator.Application.Mediator.Queries.StreamQuery;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaveApis.Common.Application.DI;
using SaveApis.Common.Application.Helper;
using SaveApis.Common.Infrastructure.Extensions;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

var assemblyHelper = new AssemblyHelper(Assembly.GetExecutingAssembly());
builder.ConfigureContainer(new AutofacServiceProviderFactory(), containerBuilder =>
{
    containerBuilder.WithModule<EfCoreModule>(null, assemblyHelper);
    containerBuilder.WithModule<MediatorModule>(null, assemblyHelper);

    containerBuilder.WithModule<SerilogModule>(null, builder.Configuration);
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

// Normal Query
var normalQueryResult = await mediator.Send(new NormalQueryQuery()).ConfigureAwait(false);
logger.Information(normalQueryResult.Value);

// Stream Query
await foreach (var item in mediator.CreateStream(new StreamQueryQuery()))
{
    logger.Information(item.Value);
}

await app.RunAsync().ConfigureAwait(false);
