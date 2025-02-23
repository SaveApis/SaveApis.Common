namespace SaveApis.Common.Infrastructure.Helper;

public interface ITypeHelper
{
    IEnumerable<Type> GetTypes();
    IEnumerable<Type> ByAttribute<TAttribute>() where TAttribute : Attribute;
}
