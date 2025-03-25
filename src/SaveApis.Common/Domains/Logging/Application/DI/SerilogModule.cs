using System.Globalization;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaveApis.Common.Domains.Core.Infrastructure.DI;
using SaveApis.Common.Domains.Logging.Domain.Types;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace SaveApis.Common.Domains.Logging.Application.DI;

public class SerilogModule(IConfiguration configuration) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        var outputs = (configuration["logging_outputs"] ?? "console").Split(',')
            .Select(it => Enum.TryParse<LoggingOutput>(it, true, out var output) ? output : LoggingOutput.Console)
            .Distinct()
            .ToList()
            .AsReadOnly();

        var collection = new ServiceCollection();

        collection.AddSerilog((_, loggerConfiguration) =>
        {
            loggerConfiguration.Enrich.FromLogContext();
            loggerConfiguration.Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Server.Kestrel"));
            loggerConfiguration.Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Hosting.Diagnostics"));
            RegisterOutput(loggerConfiguration, outputs, LoggingOutput.Console, RegisterConsoleOutput);
            RegisterOutput(loggerConfiguration, outputs, LoggingOutput.File, RegisterFileOutput);
            RegisterOutput(loggerConfiguration, outputs, LoggingOutput.Elasticsearch, RegisterElasticsearchOutput);
        });

        builder.Populate(collection);
    }

    private static void RegisterOutput(LoggerConfiguration loggerConfiguration, IReadOnlyCollection<LoggingOutput> outputs, LoggingOutput output,
        Action<LoggerConfiguration> loggerConfigurationAction)
    {
        if (!outputs.Contains(output))
        {
            return;
        }

        loggerConfigurationAction(loggerConfiguration);
    }

    private static void RegisterConsoleOutput(LoggerConfiguration loggerConfiguration)
    {
        loggerConfiguration.WriteTo.Console(LogEventLevel.Information, formatProvider: CultureInfo.InvariantCulture).MinimumLevel.Information();
    }

    private void RegisterFileOutput(LoggerConfiguration loggerConfiguration)
    {
        var path = configuration["logging_file_path"] ?? "logs/log-.txt";
        loggerConfiguration.WriteTo.File(path, formatProvider: CultureInfo.InvariantCulture, rollingInterval: RollingInterval.Day).MinimumLevel.Verbose();
    }

    private void RegisterElasticsearchOutput(LoggerConfiguration loggerConfiguration)
    {
        var now = DateTime.UtcNow;
        var uri = configuration["logging_elasticsearch_uri"] ?? "http://localhost:9200";
        var indexName = configuration["logging_elasticsearch_index"] ?? "saveapis-common";

        var options = new ElasticsearchSinkOptions(new DistributedTransport(new TransportConfiguration(new Uri(uri))))
        {
            BootstrapMethod = BootstrapMethod.Failure,
            DataStream = new DataStreamName(indexName, $"{now.Year}-{now.Month}-{now.Day}"),
        };

        loggerConfiguration.WriteTo.Elasticsearch(options).MinimumLevel.Verbose();
    }
}
