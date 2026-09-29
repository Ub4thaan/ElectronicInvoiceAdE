using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

/// <summary>
/// Official payment terms (<CondizioniPagamento>) per Agenzia delle Entrate Technical Specifications v1.9.1.
/// </summary>
public static class PaymentTerms
{
    public const string Installments = "TP01";
    public const string Full = "TP02";
    public const string Advance = "TP03";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [Installments] = new(Installments, "Installments", "Payment in installments", "Pagamento a rate"),
        [Full] = new(Full, "Full", "Full payment in single settlement", "Pagamento completo"),
        [Advance] = new(Advance, "Advance", "Advance payment", "Anticipo")
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
