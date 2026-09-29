using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ElectronicInvoiceAdE.Validation;

/// <summary>
/// Mathematical and formal validation algorithms for Italian tax codes and VAT IDs.
/// </summary>
public static class ItalianTaxValidation
{
    private static readonly int[] OddConversion =
    {
        1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 1, 0, 5, 7, 9, 13, 15, 17, 19, 21,
        2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23
    };

    private static readonly Regex CodiceFiscalePersonRegex = new(@"^[A-Z]{6}[0-9LMNPQRSTUV]{2}[A-EHLMPR-T][0-9LMNPQRSTUV]{2}[A-Z][0-9LMNPQRSTUV]{3}[A-Z]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Validates an Italian Partita IVA (11 numeric digits with Luhn checksum).
    /// </summary>
    public static bool ValidatePartitaIva(string? piva)
    {
        if (string.IsNullOrWhiteSpace(piva))
            return false;

        piva = piva!.Trim();
        if (piva.Length != 11 || !piva.All(char.IsDigit))
            return false;

        var sum = 0;
        for (var i = 0; i < 10; i++)
        {
            var digit = piva[i] - '0';
            if (i % 2 == 0)
            {
                // Odd position (1st, 3rd, 5th... 0-indexed: 0, 2, 4...)
                sum += digit;
            }
            else
            {
                // Even position (2nd, 4th... 0-indexed: 1, 3, 5...)
                var doubled = digit * 2;
                sum += doubled > 9 ? doubled - 9 : doubled;
            }
        }

        var expectedCheckDigit = (10 - (sum % 10)) % 10;
        var actualCheckDigit = piva[10] - '0';

        return expectedCheckDigit == actualCheckDigit;
    }

    /// <summary>
    /// Validates an Italian Codice Fiscale (either 16-character alphanumeric for persons or 11-digit numeric for legal entities).
    /// </summary>
    public static bool ValidateCodiceFiscale(string? cf)
    {
        if (string.IsNullOrWhiteSpace(cf))
            return false;

        cf = cf!.Trim().ToUpperInvariant();

        // 11-digit legal entity (matches Partita IVA algorithm)
        if (cf.Length == 11 && cf.All(char.IsDigit))
            return ValidatePartitaIva(cf);

        // 16-character natural person
        if (cf.Length != 16 || !CodiceFiscalePersonRegex.IsMatch(cf))
            return false;

        var sum = 0;
        for (var i = 0; i < 15; i++)
        {
            var c = cf[i];
            var val = char.IsDigit(c) ? c - '0' : c - 'A' + 10;

            if (i % 2 == 0) // Odd position (1st, 3rd, 5th...)
            {
                sum += OddConversion[val];
            }
            else // Even position (2nd, 4th, 6th...)
            {
                sum += char.IsDigit(c) ? c - '0' : c - 'A';
            }
        }

        var expectedChar = (char)('A' + (sum % 26));
        return cf[15] == expectedChar;
    }
}
