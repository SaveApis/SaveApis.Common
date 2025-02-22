using System.Reflection;
using SaveApis.Common.Infrastructure.Helper;

namespace SaveApis.Common.Application.Helper;

public class AssemblyHelper : IAssemblyHelper
{
    private readonly ICollection<Assembly> _assemblies;

    public AssemblyHelper(Assembly callingAssembly)
    {
        _assemblies = [];

        AddAssemblies(Assembly.GetExecutingAssembly(), callingAssembly);
    }

    public IEnumerable<Assembly> GetAssemblies()
    {
        return _assemblies;
    }

    public IAssemblyHelper AddAssembly(Assembly assembly)
    {
        if (_assemblies.Contains(assembly))
        {
            return this;
        }

        _assemblies.Add(assembly);

        return this;
    }

    public IAssemblyHelper AddAssemblies(params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            AddAssembly(assembly);
        }

        return this;
    }
}
