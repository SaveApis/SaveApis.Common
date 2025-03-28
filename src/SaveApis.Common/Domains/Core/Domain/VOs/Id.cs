using Vogen;

namespace SaveApis.Common.Domains.Core.Domain.VOs;

[ValueObject<Guid>(conversions: Conversions.NewtonsoftJson | Conversions.EfCoreValueConverter)]
public partial class Id
{
    private static Validation Validate(Guid input)
    {
        return input == Guid.Empty
            ? Validation.Invalid("Id cannot be empty")
            : Validation.Ok;
    }

    private static Guid NormalizeInput(Guid input)
    {
        return input;
    }

    public static Id Generate()
    {
        return From(Guid.NewGuid());
    }
}
