using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

/// <summary>
/// VAT collectibility options (<EsigibilitaIVA>).
/// </summary>
public static class VatCollectibility
{
    public const string Immediate = "I";
    public const string Deferred = "D";
    public const string SplitPayment = "S";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [Immediate] = new(Immediate, "Immediate", "Immediate VAT collectibility", "Esigibilità immediata"),
        [Deferred] = new(Deferred, "Deferred", "Deferred VAT collectibility", "Esigibilità differita"),
        [SplitPayment] = new(SplitPayment, "SplitPayment", "Split payment (scissione dei pagamenti per la P.A.)", "Scissione dei pagamenti")
    };

    public static IReadOnlyCollection<LookupCode> All => Items.Values.ToList().AsReadOnly();

    public static bool IsValid(string? code) => code != null && Items.ContainsKey(code);

    public static LookupCode? TryGet(string? code)
    {
        if (code != null && Items.TryGetValue(code, out var item))
            return item;
        return null;
    }
}
