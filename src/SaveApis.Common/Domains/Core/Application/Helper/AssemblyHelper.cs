using System.Reflection;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;

namespace SaveApis.Common.Domains.Core.Application.Helper;

public class AssemblyHelper : IAssemblyHelper
{
    private readonly IList<Assembly> _assemblies = [];

    public AssemblyHelper(Assembly callerAssembly)
    {
        RegisterAssemblies(Assembly.GetExecutingAssembly(), callerAssembly);
    }

    public IAssemblyHelper RegisterAssembly(Assembly assembly)
    {
        if (_assemblies.Contains(assembly))
        {
            return this;
        }

        _assemblies.Add(assembly);

        return this;
    }

    public IAssemblyHelper RegisterAssemblies(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            RegisterAssembly(assembly);
        }

        return this;
    }

    public IAssemblyHelper RegisterAssemblies(params Assembly[] assemblies)
    {
        return RegisterAssemblies(assemblies.ToList());
    }

    public IReadOnlyCollection<Assembly> GetRegisteredAssemblies()
    {
        return _assemblies.AsReadOnly();
    }
}
