using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaveApis.Common.Domains.Core.Application.Helper;
using SaveApis.Common.Domains.Core.Infrastructure.Extensions;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Extensions;

var builder = Host.CreateApplicationBuilder(args);

var hangfireType = builder.Configuration.GetHangfireType();
var assemblyHelper = new AssemblyHelper(Assembly.GetExecutingAssembly());
builder.ConfigureContainer(new AutofacServiceProviderFactory(), containerBuilder => containerBuilder.WithCommonModules(assemblyHelper, builder.Configuration, hangfireType));

var app = builder.Build();

await app.RunAsync().ConfigureAwait(false);
