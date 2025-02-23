using System.Text.RegularExpressions;
using Vogen;
using Generator = PasswordGenerator.Password;

namespace SaveApis.Common.Domain.VO;

[ValueObject<string>(conversions: Conversions.NewtonsoftJson | Conversions.EfCoreValueConverter)]
public partial class Password
{
    public Hash ToHash()
    {
        return Hash.From(BCrypt.Net.BCrypt.HashPassword(Value));
    }

    public static Password Generate()
    {
        var generator = new Generator(true, true, true, true, 16);

        string password;
        do
        {
            password = generator.Next();
        }
        while (string.IsNullOrWhiteSpace(password));

        return From(password);
    }

    private static string NormalizeInput(string input)
    {
        return input;
    }

    private static Validation Validate(string input)
    {
        var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{16,}$");
        return string.IsNullOrWhiteSpace(input)
            ? Validation.Invalid("Password cannot be empty")
            : regex.IsMatch(input)
                ? Validation.Ok
                : Validation.Invalid("Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 digit, 1 special character and be at least 16 characters long");
    }
}
