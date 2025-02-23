using Vogen;

namespace SaveApis.Common.Domain.VO;

[ValueObject<string>(conversions: Conversions.NewtonsoftJson | Conversions.EfCoreValueConverter)]
public partial class Hash
{
    public bool Verify(Password password)
    {
        return BCrypt.Net.BCrypt.Verify(password.Value, Value);
    }

    private static string NormalizeInput(string input)
    {
        return input;
    }

    private static Validation Validate(string input)
    {
        return string.IsNullOrWhiteSpace(input)
            ? Validation.Invalid("Hash cannot be empty")
            : input.Length != 60
                ? Validation.Invalid("Hash must be 60 characters long")
                : Validation.Ok;
    }
}
