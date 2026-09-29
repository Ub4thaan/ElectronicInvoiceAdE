using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

/// <summary>
/// Transmission formats (<FormatoTrasmissione> and root versione attribute).
/// </summary>
public static class TransmissionFormat
{
    public const string PublicAdministration = "FPA12";
    public const string PrivateParties = "FPR12";
    public const string Simplified = "FSM10";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [PublicAdministration] = new(PublicAdministration, "PublicAdministration", "Invoice towards Public Administrations (FPA12)", "Fattura verso PA"),
        [PrivateParties] = new(PrivateParties, "PrivateParties", "Invoice towards private entities B2B/B2C (FPR12)", "Fattura verso privati"),
        [Simplified] = new(Simplified, "Simplified", "Simplified invoice (FSM10)", "Fattura semplificata")
    };

    public static IReadOnlyCollection<LookupCode> All => Items.Values.ToList().AsReadOnly();

    public static bool IsValid(string? code) => code != null && Items.ContainsKey(code);

    public static bool IsOrdinary(string? code) => code is PublicAdministration or PrivateParties;

    public static bool IsSimplified(string? code) => code == Simplified;

    public static LookupCode? TryGet(string? code)
    {
        if (code != null && Items.TryGetValue(code, out var item))
            return item;
        return null;
    }
}
