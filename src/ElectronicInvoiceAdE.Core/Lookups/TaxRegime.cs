using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

/// <summary>
/// Official tax regimes (<RegimeFiscale>) per Agenzia delle Entrate Technical Specifications v1.9.1.
/// </summary>
public static class TaxRegime
{
    public const string Ordinary = "RF01";
    public const string MinimumTaxpayers = "RF02";
    public const string AgricultureAndFishing = "RF04";
    public const string TobaccoSales = "RF05";
    public const string MatchSales = "RF06";
    public const string Publishing = "RF07";
    public const string PublicTelephoneManagement = "RF08";
    public const string PublicTransportResale = "RF09";
    public const string EntertainmentAndGaming = "RF10";
    public const string TravelAndTourismAgencies = "RF11";
    public const string FarmHolidays = "RF12";
    public const string DoorToDoorSales = "RF13";
    public const string ResaleOfUsedGoods = "RF14";
    public const string ArtworkAuctionAgencies = "RF15";
    public const string VatPaidInCashByPublicAdministration = "RF16";
    public const string VatPaidInCash = "RF17";
    public const string Other = "RF18";
    public const string FlatRate = "RF19";
    public const string CrossBorderVatExemption = "RF20";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [Ordinary] = new(Ordinary, "Ordinary", "Ordinary tax regime", "Regime ordinario"),
        [MinimumTaxpayers] = new(MinimumTaxpayers, "MinimumTaxpayers", "Minimum taxpayers regime (art. 1 c. 96-117, L. 244/2007)", "Regime contribuenti minimi"),
        [AgricultureAndFishing] = new(AgricultureAndFishing, "AgricultureAndFishing", "Agriculture and related activities and fishing (art. 34 and 34-bis)", "Agricoltura e attività connesse e pesca"),
        [TobaccoSales] = new(TobaccoSales, "TobaccoSales", "Sale of salts and tobacco (art. 74 c. 1)", "Vendita sali e tabacchi"),
        [MatchSales] = new(MatchSales, "MatchSales", "Trade of matches (art. 74 c. 1)", "Commercio dei fiammiferi"),
        [Publishing] = new(Publishing, "Publishing", "Publishing (art. 74 c. 1)", "Editoria"),
        [PublicTelephoneManagement] = new(PublicTelephoneManagement, "PublicTelephoneManagement", "Management of public telephone services (art. 74 c. 1)", "Gestione di servizi di telefonia pubblica"),
        [PublicTransportResale] = new(PublicTransportResale, "PublicTransportResale", "Resale of public transport and parking documents (art. 74 c. 1)", "Rivendita documenti di trasporto pubblico e di sosta"),
        [EntertainmentAndGaming] = new(EntertainmentAndGaming, "EntertainmentAndGaming", "Entertainment, gaming and other activities (art. 74 c. 6)", "Intrattenimenti, giochi e altre attività"),
        [TravelAndTourismAgencies] = new(TravelAndTourismAgencies, "TravelAndTourismAgencies", "Travel and tourism agencies (art. 74-ter)", "Agenzie di viaggi e turismo"),
        [FarmHolidays] = new(FarmHolidays, "FarmHolidays", "Farm holidays / agritourism (art. 5 c. 2, L. 413/1991)", "Agriturismo"),
        [DoorToDoorSales] = new(DoorToDoorSales, "DoorToDoorSales", "Door-to-door sales (art. 25-bis c. 6, DPR 600/1973)", "Vendite a domicilio"),
        [ResaleOfUsedGoods] = new(ResaleOfUsedGoods, "ResaleOfUsedGoods", "Resale of used goods, artworks, antiques (art. 36, DL 41/1995)", "Rivendita beni usati, oggetti d'arte, d'antiquariato o da collezione"),
        [ArtworkAuctionAgencies] = new(ArtworkAuctionAgencies, "ArtworkAuctionAgencies", "Artwork, antiques or collector's items auction agencies (art. 40-bis, DL 41/1995)", "Agenzie di vendite all'asta di oggetti d'arte, antiquariato o collezione"),
        [VatPaidInCashByPublicAdministration] = new(VatPaidInCashByPublicAdministration, "VatPaidInCashByPublicAdministration", "VAT paid in cash by P.A. (art. 6 c. 5, DPR 633/1972)", "IVA per cassa P.A."),
        [VatPaidInCash] = new(VatPaidInCash, "VatPaidInCash", "VAT paid in cash by subjects with turnover below 200,000 EUR (art. 7, DL 185/2008)", "IVA per cassa"),
        [Other] = new(Other, "Other", "Other tax regime", "Altro"),
        [FlatRate] = new(FlatRate, "FlatRate", "Flat-rate tax regime / forfettario (art. 1 c. 54-89, L. 190/2014)", "Regime forfettario"),
        [CrossBorderVatExemption] = new(CrossBorderVatExemption, "CrossBorderVatExemption", "Cross-border VAT exemption regime (EU Directive 2020/285)", "Regime transfrontaliero di franchigia IVA (Direttiva UE 2020/285)")
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
