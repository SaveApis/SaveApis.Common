using Vogen;

namespace SaveApis.Common.Domain.VO;

[ValueObject<string>(conversions: Conversions.NewtonsoftJson | Conversions.EfCoreValueConverter)]
public partial class Description
{
    public static readonly Description Empty = new Description(string.Empty);

    private static string NormalizeInput(string input)
    {
        return input.Trim();
    }
    private static Validation Validate(string input)
    {
        return string.IsNullOrWhiteSpace(input)
            ? Validation.Invalid("Description cannot be empty")
            : input.Length > 1000
                ? Validation.Invalid("Description cannot be longer than 1000 characters")
                : Validation.Ok;
    }
}
