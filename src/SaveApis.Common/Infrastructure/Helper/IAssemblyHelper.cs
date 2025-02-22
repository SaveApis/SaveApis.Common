using System.Reflection;

namespace SaveApis.Common.Infrastructure.Helper;

public interface IAssemblyHelper
{
    IEnumerable<Assembly> GetAssemblies();

    IAssemblyHelper AddAssembly(Assembly assembly);
    IAssemblyHelper AddAssemblies(params Assembly[] assemblies);
}
