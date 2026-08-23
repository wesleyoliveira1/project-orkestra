using System.Text.RegularExpressions;

namespace ProjectOrkestra.Domain.Validators;

public static partial class EmailValidator
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    public static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return EmailRegex().IsMatch(value);
    }
}
