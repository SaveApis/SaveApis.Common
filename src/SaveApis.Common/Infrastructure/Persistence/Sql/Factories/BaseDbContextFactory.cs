using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace SaveApis.Common.Infrastructure.Persistence.Sql.Factories;

public class BaseDbContextFactory<TContext>(IConfiguration configuration, IMediator mediator) : IDesignTimeDbContextFactory<TContext> where TContext : BaseDbContext
{
    public TContext CreateDbContext(string[] args)
    {
        try
        {
            var options = BuildOptions();

            return (TContext)Activator.CreateInstance(typeof(TContext), options, mediator)!;
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"An error occurred creating the DB context {typeof(TContext).Name}. ({e.Message})", e);
        }
    }

    private DbContextOptions<TContext> BuildOptions()
    {
        var connectionString = BuildConnectionString();
        var builder = new DbContextOptionsBuilder<TContext>();
        builder.UseMySql(connectionString, MySqlServerVersion.LatestSupportedServerVersion, optionsBuilder =>
        {
            optionsBuilder.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}_{table}");
            optionsBuilder.DisableBackslashEscaping();
            optionsBuilder.EnablePrimitiveCollectionsSupport();
            optionsBuilder.EnableStringComparisonTranslations();
            optionsBuilder.EnableIndexOptimizedBooleanColumns();
            optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            optionsBuilder.UseNewtonsoftJson();
        });

        return builder.Options;
    }

    private string BuildConnectionString()
    {
        var name = configuration["database_sql_name"] ?? string.Empty;
        var server = configuration["database_sql_server"] ?? string.Empty;
        var port = configuration["database_sql_port"] ?? string.Empty;
        var database = configuration["database_sql_database"] ?? string.Empty;
        var user = configuration["database_sql_user"] ?? string.Empty;
        var password = configuration["database_sql_password"] ?? string.Empty;

        var builder = new MySqlConnectionStringBuilder
        {
            ApplicationName = name,
            Server = server,
            Pooling = true,
            Port = uint.TryParse(port, out var p) ? p : 3306,
            Database = database,
            UserID = user,
            Password = password,
            Pipelining = true,
            UseCompression = true,
            AllowUserVariables = true,
            BrowsableConnectionString = false,
            UseAffectedRows = true,
        };

        return builder.ConnectionString;
    }
}
