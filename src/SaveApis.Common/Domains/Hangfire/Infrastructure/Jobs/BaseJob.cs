using Hangfire.Console;
using Hangfire.Server;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Configurations;
using SaveApis.Common.Domains.Hangfire.Infrastructure.Events;
using Serilog;
using Serilog.Events;

namespace SaveApis.Common.Domains.Hangfire.Infrastructure.Jobs;

public abstract class BaseJob<TEvent, TConfiguration>(ILogger logger) : IJob<TEvent> where TEvent : IEvent where TConfiguration : IJobConfiguration
{
    private TConfiguration? _configuration;

    protected TConfiguration Configuration => _configuration ?? throw new InvalidOperationException("Job configuration is not set");
    public void ApplyConfiguration(IJobConfiguration configuration)
    {
        if (configuration is not TConfiguration typedConfiguration)
        {
            throw new ArgumentException($"Configuration type mismatch. Expected {typeof(TConfiguration).Name}, but got {configuration.GetType().Name}");
        }

        _configuration = typedConfiguration;
    }

    protected virtual bool CheckSupport(TEvent @event)
    {
        return true;
    }
    public virtual Task<bool> CheckSupportAsync(TEvent @event, CancellationToken cancellationToken)
    {
        return Task.FromResult(CheckSupport(@event));
    }

    public abstract Task RunAsync(TEvent @event, PerformContext? performContext, CancellationToken cancellationToken);

    protected void Log(LogEventLevel level, string message, PerformContext? performContext, Exception? exception = null)
    {
        logger.Write(level, exception, message);
        LogPerformContext(level, message, performContext, exception);
    }

    private static void LogPerformContext(LogEventLevel level, string message, PerformContext? performContext, Exception? exception)
    {
        var consoleColor = level switch
        {
            LogEventLevel.Verbose => ConsoleTextColor.White,
            LogEventLevel.Debug => ConsoleTextColor.White,
            LogEventLevel.Information => ConsoleTextColor.Green,
            LogEventLevel.Warning => ConsoleTextColor.Yellow,
            LogEventLevel.Error => ConsoleTextColor.Red,
            LogEventLevel.Fatal => ConsoleTextColor.DarkRed,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null),
        };

        performContext?.WriteLine(consoleColor, message);
        if (exception is not null)
        {
            performContext?.WriteLine(consoleColor, exception.ToString());
        }
    }
}
