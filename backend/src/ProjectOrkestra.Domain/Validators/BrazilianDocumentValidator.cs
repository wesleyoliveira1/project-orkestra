using System.Text.RegularExpressions;

namespace ProjectOrkestra.Domain.Validators;

public static class BrazilianDocumentValidator
{
    public static bool IsValidCpf(string value)
    {
        string digits = NormalizeCpf(value);

        if (!HasExpectedLength(digits, 11) || HasRepeatedDigits(digits))
            return false;

        return true;
    }

    public static string FormatCpf(string value)
    {
        string digits = NormalizeCpf(value);

        if (!IsValidCpf(value))
            throw new ArgumentException("Invalid CPF.", nameof(value));

        return $"{digits[..3]}.{digits[3..6]}.{digits[6..9]}-{digits[9..]}";
    }

    public static bool IsValidCnpj(string value)
    {
        string normalizedValue = NormalizeCnpj(value);

        if (!HasExpectedLength(normalizedValue, 14) || HasRepeatedCharacters(normalizedValue))
            return false;

        if (!normalizedValue[..12].All(char.IsLetterOrDigit) || !normalizedValue[12..].All(char.IsDigit))
            return false;

        return true;
    }

    public static string FormatCnpj(string value)
    {
        string normalizedValue = NormalizeCnpj(value);

        if (!IsValidCnpj(value))
            throw new ArgumentException("Invalid CNPJ.", nameof(value));

        return $"{normalizedValue[..2]}.{normalizedValue[2..5]}.{normalizedValue[5..8]}/{normalizedValue[8..12]}-{normalizedValue[12..]}";
    }

    public static bool IsValidBrazilianPhone(string value)
    {
        string digits = NormalizePhone(value);

        if (digits.StartsWith("55") && digits.Length is 12 or 13)
            digits = digits[2..];

        if (digits.Length is not (10 or 11) || digits[0] == '0')
            return false;

        string areaCode = digits[..2];
        string subscriberNumber = digits[2..];

        return areaCode.All(digit => digit is >= '1' and <= '9')
            && subscriberNumber.Length is 8 or 9
            && subscriberNumber[0] != '0';
    }

    public static string FormatBrazilianPhone(string value)
    {
        string digits = NormalizePhone(value);

        if (digits.StartsWith("55") && digits.Length is 12 or 13)
            digits = digits[2..];

        if (!IsValidBrazilianPhone(value))
            throw new ArgumentException("Invalid Brazilian phone number.", nameof(value));

        return digits.Length == 11
            ? $"({digits[..2]}) {digits[2..7]}-{digits[7..]}"
            : $"({digits[..2]}) {digits[2..6]}-{digits[6..]}";
    }

    private static string NormalizeCpf(string value)
    {
        return OnlyDigits(value);
    }

    private static string NormalizeCnpj(string value)
    {
        return Regex
            .Replace(value ?? string.Empty, "[^A-Za-z0-9]", string.Empty)
            .ToUpperInvariant();
    }

    private static string NormalizePhone(string value)
    {
        return OnlyDigits(value);
    }

    private static string OnlyDigits(string value)
    {
        return Regex.Replace(value ?? string.Empty, "[^0-9]", string.Empty);
    }

    private static bool HasExpectedLength(string value, int length)
    {
        return value.Length == length;
    }

    private static bool HasRepeatedDigits(string value)
    {
        return value.All(digit => digit == value[0]);
    }

    private static bool HasRepeatedCharacters(string value)
    {
        return value.All(character => character == value[0]);
    }
}