using Microsoft.Extensions.Configuration;
using SaveApis.Common.Domains.Hangfire.Domain.Types;

namespace SaveApis.Common.Domains.Hangfire.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static HangfireType GetHangfireType(this IConfiguration configuration)
    {
        var type = configuration["hangfire_type"];

        return Enum.TryParse<HangfireType>(type, true, out var hangfireType)
            ? hangfireType
            : HangfireType.Unknown;
    }
}
