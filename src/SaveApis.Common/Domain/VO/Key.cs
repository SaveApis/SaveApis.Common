using Vogen;

namespace SaveApis.Common.Domain.VO;

[ValueObject<string>(conversions: Conversions.NewtonsoftJson | Conversions.EfCoreValueConverter)]
public partial class Key
{
    public static readonly Key Empty = new Key(string.Empty);

    private static string NormalizeInput(string input)
    {
        return input.ToLowerInvariant().Replace(' ', '_').Trim();
    }
    private static Validation Validate(string input)
    {
        return string.IsNullOrWhiteSpace(input)
            ? Validation.Invalid("Key cannot be empty")
            : input.Length > 50
                ? Validation.Invalid("Key cannot be longer than 50 characters")
                : Validation.Ok;
    }
}
