using System.Reflection;
using SaveApis.Common.Domains.Core.Infrastructure.Helper;

namespace SaveApis.Common.Domains.Core.Application.Helper;

public class TypeHelper(IAssemblyHelper assemblyHelper) : ITypeHelper
{
    public IReadOnlyCollection<Type> GetTypes()
    {
        return [.. assemblyHelper.GetRegisteredAssemblies().SelectMany(assembly => assembly.GetTypes())];
    }

    public IReadOnlyCollection<Type> GetTypesByAttribute<TAttribute>() where TAttribute : Attribute
    {
        return [.. GetTypes().Where(type => type.GetCustomAttribute<TAttribute>() is not null)];
    }
}
