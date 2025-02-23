using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;

namespace SaveApis.Common.Infrastructure.Hangfire.Attributes;

public class HangfireJobNameAttribute(string displayName) : JobDisplayNameAttribute(displayName)
{
    public override string Format(DashboardContext context, Job job)
    {
        var format = base.Format(context, job);

        var prefix = ReadPrefix(job, "Domains");
        prefix ??= ReadPrefix(job, "Integrations");
        prefix ??= "Core";

        return $"[{prefix}] {format}";
    }
    private static string? ReadPrefix(Job job, string identifier)
    {
        if (!job.Type.Namespace?.Contains(identifier, StringComparison.InvariantCultureIgnoreCase) ?? true)
        {
            return null;
        }

        var parts = (job.Type.Namespace ?? string.Empty).Split('.');
        var index = Array.IndexOf(parts, identifier);
        if (index == -1 || index + 1 >= parts.Length)
        {
            return null;
        }

        return parts[index + 1];
    }
}
