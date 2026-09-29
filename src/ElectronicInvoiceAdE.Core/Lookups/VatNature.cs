using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

/// <summary>
/// Official VAT nature codes (<Natura>) per Agenzia delle Entrate Technical Specifications v1.9.1.
/// Used when VAT rate is 0% to indicate the reason of non-taxability/exemption.
/// </summary>
public static class VatNature
{
    public const string ExcludedArt15 = "N1";
    public const string NotSubjectArticles7To7Septies = "N2.1";
    public const string NotSubjectOther = "N2.2";
    public const string NonTaxableExport = "N3.1";
    public const string NonTaxableIntraCommunity = "N3.2";
    public const string NonTaxableSanMarino = "N3.3";
    public const string NonTaxableTreatedAsExport = "N3.4";
    public const string NonTaxableDeclarationOfIntent = "N3.5";
    public const string NonTaxableOtherNoPlafond = "N3.6";
    public const string Exempt = "N4";
    public const string MarginRegimeVatNotExposed = "N5";
    public const string ReverseChargeScrap = "N6.1";
    public const string ReverseChargeGoldSilver = "N6.2";
    public const string ReverseChargeSubcontractingConstruction = "N6.3";
    public const string ReverseChargeBuildings = "N6.4";
    public const string ReverseChargeMobilePhones = "N6.5";
    public const string ReverseChargeElectronicProducts = "N6.6";
    public const string ReverseChargeConstructionSector = "N6.7";
    public const string ReverseChargeEnergySector = "N6.8";
    public const string ReverseChargeOther = "N6.9";
    public const string VatPaidInOtherEuCountry = "N7";

    // Legacy simplified top-level codes
    public const string NotSubjectGeneric = "N2";
    public const string NonTaxableGeneric = "N3";
    public const string ReverseChargeGeneric = "N6";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [ExcludedArt15] = new(ExcludedArt15, "ExcludedArt15", "Excluded pursuant to Art. 15", "Escluse ex art. 15"),
        [NotSubjectArticles7To7Septies] = new(NotSubjectArticles7To7Septies, "NotSubjectArticles7To7Septies", "Not subject to VAT under articles 7 to 7-septies of DPR 633/72", "Non soggette ad IVA ai sensi degli artt. da 7 a 7-septies"),
        [NotSubjectOther] = new(NotSubjectOther, "NotSubjectOther", "Not subject – other cases", "Non soggette – altri casi"),
        [NonTaxableExport] = new(NonTaxableExport, "NonTaxableExport", "Non-taxable - exportations", "Non imponibili - esportazioni"),
        [NonTaxableIntraCommunity] = new(NonTaxableIntraCommunity, "NonTaxableIntraCommunity", "Non-taxable - intra-Community transfers", "Non imponibili - cessioni intracomunitarie"),
        [NonTaxableSanMarino] = new(NonTaxableSanMarino, "NonTaxableSanMarino", "Non-taxable - transfers to San Marino", "Non imponibili - cessioni verso San Marino"),
        [NonTaxableTreatedAsExport] = new(NonTaxableTreatedAsExport, "NonTaxableTreatedAsExport", "Non-taxable - transactions treated as export supplies", "Non imponibili - operazioni assimilate alle cessioni all'esportazione"),
        [NonTaxableDeclarationOfIntent] = new(NonTaxableDeclarationOfIntent, "NonTaxableDeclarationOfIntent", "Non-taxable - following declaration of intent", "Non imponibili - a seguito di dichiarazioni d'intento"),
        [NonTaxableOtherNoPlafond] = new(NonTaxableOtherNoPlafond, "NonTaxableOtherNoPlafond", "Non-taxable – other transactions not contributing to ceiling", "Non imponibili – altre operazioni che non concorrono al plafond"),
        [Exempt] = new(Exempt, "Exempt", "Exempt from VAT", "Esenti"),
        [MarginRegimeVatNotExposed] = new(MarginRegimeVatNotExposed, "MarginRegimeVatNotExposed", "Margin regime / VAT not exposed on invoice", "Regime del margine / IVA non esposta in fattura"),
        [ReverseChargeScrap] = new(ReverseChargeScrap, "ReverseChargeScrap", "Reverse charge - scrap and recyclable materials", "Inversione contabile - rottami e altri materiali di recupero"),
        [ReverseChargeGoldSilver] = new(ReverseChargeGoldSilver, "ReverseChargeGoldSilver", "Reverse charge - gold and silver transfers", "Inversione contabile - cessione di oro e argento"),
        [ReverseChargeSubcontractingConstruction] = new(ReverseChargeSubcontractingConstruction, "ReverseChargeSubcontractingConstruction", "Reverse charge - subcontracting in construction sector", "Inversione contabile - subappalto nel settore edile"),
        [ReverseChargeBuildings] = new(ReverseChargeBuildings, "ReverseChargeBuildings", "Reverse charge - transfer of buildings", "Inversione contabile - cessione di fabbricati"),
        [ReverseChargeMobilePhones] = new(ReverseChargeMobilePhones, "ReverseChargeMobilePhones", "Reverse charge - transfer of mobile phones", "Inversione contabile - cessione di telefoni cellulari"),
        [ReverseChargeElectronicProducts] = new(ReverseChargeElectronicProducts, "ReverseChargeElectronicProducts", "Reverse charge - transfer of electronic products", "Inversione contabile - cessione di prodotti elettronici"),
        [ReverseChargeConstructionSector] = new(ReverseChargeConstructionSector, "ReverseChargeConstructionSector", "Reverse charge - provisions in construction and related sectors", "Inversione contabile - prestazioni settore edile"),
        [ReverseChargeEnergySector] = new(ReverseChargeEnergySector, "ReverseChargeEnergySector", "Reverse charge - transactions in the energy sector", "Inversione contabile - operazioni settore energetico"),
        [ReverseChargeOther] = new(ReverseChargeOther, "ReverseChargeOther", "Reverse charge - other cases", "Inversione contabile - altri casi"),
        [VatPaidInOtherEuCountry] = new(VatPaidInOtherEuCountry, "VatPaidInOtherEuCountry", "VAT paid in another EU country", "Imposta assolta in altro stato UE"),

        [NotSubjectGeneric] = new(NotSubjectGeneric, "NotSubjectGeneric", "Not subject to VAT (legacy/simplified)", "Non soggette ad IVA"),
        [NonTaxableGeneric] = new(NonTaxableGeneric, "NonTaxableGeneric", "Non-taxable (legacy/simplified)", "Non imponibili"),
        [ReverseChargeGeneric] = new(ReverseChargeGeneric, "ReverseChargeGeneric", "Reverse charge (legacy/simplified)", "Inversione contabile")
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
