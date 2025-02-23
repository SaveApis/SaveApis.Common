using System.ComponentModel.DataAnnotations;
using Vogen;

namespace SaveApis.Common.Domain.VO;

[ValueObject<string>(conversions: Conversions.NewtonsoftJson | Conversions.EfCoreValueConverter)]
public partial class Email
{
    public static readonly Email Empty = new Email(string.Empty);

    private static EmailAddressAttribute Attribute { get; } = new EmailAddressAttribute();

    private static string NormalizeInput(string input)
    {
        return input.Trim();
    }
    private static Validation Validate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Validation.Invalid("Email cannot be empty");
        }

        if (!Attribute.IsValid(input))
        {
            return Validation.Invalid("Email is invalid");
        }

        return Validation.Ok;
    }
}
