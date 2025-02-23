// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaveApis.Common.Application.Helper;
using SaveApis.Common.Infrastructure.Extensions;

var helper = new AssemblyHelper(Assembly.GetExecutingAssembly());
var builder = Host.CreateApplicationBuilder(args);

builder.ConfigureContainer(new AutofacServiceProviderFactory(), containerBuilder => containerBuilder.WithCommonModules(builder.Configuration, helper));

var app = builder.Build();

await app.RunAsync().ConfigureAwait(false);
