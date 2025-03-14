// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaveApis.Common.Application.DI;
using SaveApis.Common.Application.Helper;
using SaveApis.Common.Infrastructure.Extensions;

var builder = Host.CreateApplicationBuilder(args);

var assemblyHelper = new AssemblyHelper(Assembly.GetExecutingAssembly());
builder.ConfigureContainer(new AutofacServiceProviderFactory(), containerBuilder => containerBuilder.WithModule<EfCoreModule>(null, assemblyHelper));

var app = builder.Build();

await app.RunAsync().ConfigureAwait(false);
