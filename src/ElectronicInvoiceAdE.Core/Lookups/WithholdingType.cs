using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

/// <summary>
/// Official withholding tax types (<TipoRitenuta>) per Agenzia delle Entrate Technical Specifications v1.9.1.
/// </summary>
public static class WithholdingType
{
    public const string NaturalPersons = "RT01";
    public const string CorporateEntities = "RT02";
    public const string InpsContribution = "RT03";
    public const string EnasarcoContribution = "RT04";
    public const string EnpamContribution = "RT05";
    public const string OtherContribution = "RT06";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [NaturalPersons] = new(NaturalPersons, "NaturalPersons", "Withholding tax for natural persons", "Ritenuta persone fisiche"),
        [CorporateEntities] = new(CorporateEntities, "CorporateEntities", "Withholding tax for corporate entities", "Ritenuta persone giuridiche"),
        [InpsContribution] = new(InpsContribution, "InpsContribution", "INPS contribution", "Contributo INPS"),
        [EnasarcoContribution] = new(EnasarcoContribution, "EnasarcoContribution", "ENASARCO contribution", "Contributo ENASARCO"),
        [EnpamContribution] = new(EnpamContribution, "EnpamContribution", "ENPAM contribution", "Contributo ENPAM"),
        [OtherContribution] = new(OtherContribution, "OtherContribution", "Other social security contribution", "Altro contributo previdenziale")
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
