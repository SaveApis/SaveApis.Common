using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Common.Infrastructure.DI;
using Serilog;
using Serilog.Events;

namespace SaveApis.Common.Application.DI;

public class SerilogModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddSerilog(configuration =>
        {
            configuration.Enrich.FromLogContext();
            configuration.MinimumLevel.Verbose();
            configuration.MinimumLevel.Override("Hangfire", LogEventLevel.Warning);
            configuration.WriteTo.Console(LogEventLevel.Information, formatProvider: CultureInfo.InvariantCulture);
            configuration.WriteTo.File("Logs/log.txt", formatProvider: CultureInfo.InvariantCulture, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7);
        });

        builder.Populate(collection);
    }
}
