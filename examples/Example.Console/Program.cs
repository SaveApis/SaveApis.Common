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

var builder = Host.CreateApplicationBuilder(args);

var assemblyHelper = new AssemblyHelper(Assembly.GetExecutingAssembly());
builder.ConfigureContainer(new AutofacServiceProviderFactory(), containerBuilder =>
{
    containerBuilder.WithModule<EfCoreModule>(null, assemblyHelper);
    containerBuilder.WithModule<MediatorModule>(null, assemblyHelper);
});

var app = builder.Build();

var mediator = app.Services.CreateScope().ServiceProvider.GetRequiredService<IMediator>();

// Normal Query
var normalQueryResult = await mediator.Send(new NormalQueryQuery()).ConfigureAwait(false);
Console.WriteLine(normalQueryResult.Value);

// Stream Query
await foreach (var item in mediator.CreateStream(new StreamQueryQuery()))
{
    Console.WriteLine(item.Value);
}

await app.RunAsync().ConfigureAwait(false);
