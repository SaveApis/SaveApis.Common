using System.Reflection;
using SaveApis.Common.Infrastructure.Helper;

namespace SaveApis.Common.Application.Helper;

public class TypeHelper(IAssemblyHelper helper) : ITypeHelper
{
    public IEnumerable<Type> GetTypes()
    {
        return helper.GetAssemblies().SelectMany(a => a.GetTypes());
    }

    public IEnumerable<Type> ByAttribute<TAttribute>() where TAttribute : Attribute
    {
        var types = GetTypes();

        return types.Where(t => t.GetCustomAttribute<TAttribute>() is not null);
    }
}
