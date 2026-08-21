using System.Text.RegularExpressions;

namespace ProjectOrkestra.Domain.Validators;

public static class BrazilianDocumentValidator
{
    public static bool IsValidCpf(string value)
    {
        var digits = NormalizeCpf(value);

        if (!HasExpectedLength(digits, 11) || HasRepeatedDigits(digits))
            return false;

        var firstCheckDigit = CalculateCheckDigit(digits[..9], 10);
        var secondCheckDigit = CalculateCheckDigit(digits[..10], 11);

        return digits[9] - '0' == firstCheckDigit && digits[10] - '0' == secondCheckDigit;
    }

    public static string FormatCpf(string value)
    {
        var digits = NormalizeCpf(value);

        if (!IsValidCpf(value))
            throw new ArgumentException("Invalid CPF.", nameof(value));

        return $"{digits[..3]}.{digits[3..6]}.{digits[6..9]}-{digits[9..]}";
    }

    public static bool IsValidCnpj(string value)
    {
        var normalizedValue = NormalizeCnpj(value);

        if (!HasExpectedLength(normalizedValue, 14) || HasRepeatedCharacters(normalizedValue))
            return false;

        if (!normalizedValue[..12].All(char.IsLetterOrDigit) || !normalizedValue[12..].All(char.IsDigit))
            return false;

        var firstCheckDigit = CalculateCnpjCheckDigit(normalizedValue[..12], new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });
        var secondCheckDigit = CalculateCnpjCheckDigit(normalizedValue[..13], new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 });

        return normalizedValue[12] - '0' == firstCheckDigit && normalizedValue[13] - '0' == secondCheckDigit;
    }

    public static string FormatCnpj(string value)
    {
        var normalizedValue = NormalizeCnpj(value);

        if (!IsValidCnpj(value))
            throw new ArgumentException("Invalid CNPJ.", nameof(value));

        return $"{normalizedValue[..2]}.{normalizedValue[2..5]}.{normalizedValue[5..8]}/{normalizedValue[8..12]}-{normalizedValue[12..]}";
    }

    public static bool IsValidBrazilianPhone(string value)
    {
        var digits = NormalizePhone(value);

        if (digits.StartsWith("55") && digits.Length is 12 or 13)
            digits = digits[2..];

        if (digits.Length is not (10 or 11) || digits[0] == '0')
            return false;

        var areaCode = digits[..2];
        var subscriberNumber = digits[2..];

        return areaCode.All(digit => digit is >= '1' and <= '9')
            && (subscriberNumber.Length == 8 || subscriberNumber.Length == 9)
            && subscriberNumber[0] != '0';
    }

    public static string FormatBrazilianPhone(string value)
    {
        var digits = NormalizePhone(value);

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
        return Regex.Replace(value ?? string.Empty, "[^A-Za-z0-9]", string.Empty).ToUpperInvariant();
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

    private static int CalculateCheckDigit(string value, int startWeight)
    {
        var sum = 0;

        for (var index = 0; index < value.Length; index++)
            sum += (value[index] - '0') * (startWeight - index);

        var remainder = (sum * 10) % 11;
        return remainder == 10 ? 0 : remainder;
    }

    private static int CalculateCheckDigit(string value, int[] weights)
    {
        var sum = 0;

        for (var index = 0; index < value.Length; index++)
            sum += (value[index] - '0') * weights[index];

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }

    private static int CalculateCnpjCheckDigit(string value, int[] weights)
    {
        var sum = 0;

        for (var index = 0; index < value.Length; index++)
            sum += GetCnpjCharacterValue(value[index]) * weights[index];

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }

    private static int GetCnpjCharacterValue(char character)
    {
        return char.IsDigit(character) ? character - '0' : character - 'A' + 17;
    }
}
