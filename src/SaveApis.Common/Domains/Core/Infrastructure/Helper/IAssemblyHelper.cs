using System.Reflection;

namespace SaveApis.Common.Domains.Core.Infrastructure.Helper;

public interface IAssemblyHelper
{
    IAssemblyHelper RegisterAssembly(Assembly assembly);
    IAssemblyHelper RegisterAssemblies(IEnumerable<Assembly> assemblies);
    IAssemblyHelper RegisterAssemblies(params Assembly[] assemblies);

    IReadOnlyCollection<Assembly> GetRegisteredAssemblies();
}
