using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;

namespace SaveApis.Common.Domains.Hangfire.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class HangfireJobNameAttribute(string name) : JobDisplayNameAttribute(name)
{
    public override string Format(DashboardContext context, Job job)
    {
        var formatted = base.Format(context, job);
        var prefix = GeneratePrefix(job.Type, "Domains");
        prefix ??= GeneratePrefix(job.Type, "Integrations");
        prefix ??= "Core";

        return $"[{prefix}] {formatted}";
    }

    private static string? GeneratePrefix(Type type, string delimiter)
    {
        var namespaceParts = type.Namespace?.Split('.') ?? [];
        var index = Array.IndexOf(namespaceParts, delimiter);
        if (index == -1 || index == namespaceParts.Length - 1)
        {
            return null;
        }

        return namespaceParts[index + 1];
    }
}
