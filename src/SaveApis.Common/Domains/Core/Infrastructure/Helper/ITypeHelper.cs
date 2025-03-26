namespace SaveApis.Common.Domains.Core.Infrastructure.Helper;

public interface ITypeHelper
{
    IReadOnlyCollection<Type> GetTypes();
    IReadOnlyCollection<Type> GetTypesByAttribute<TAttribute>() where TAttribute : Attribute;
}
