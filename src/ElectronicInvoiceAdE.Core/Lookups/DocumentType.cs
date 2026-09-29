using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

/// <summary>
/// Official document types (<TipoDocumento>) per Agenzia delle Entrate Technical Specifications v1.9.1.
/// </summary>
public static class DocumentType
{
    public const string Invoice = "TD01";
    public const string AdvanceOnInvoice = "TD02";
    public const string AdvanceOnFee = "TD03";
    public const string CreditNote = "TD04";
    public const string DebitNote = "TD05";
    public const string Fee = "TD06";
    public const string SimplifiedInvoice = "TD07";
    public const string SimplifiedCreditNote = "TD08";
    public const string SimplifiedDebitNote = "TD09";
    public const string InternalReverseChargeIntegration = "TD16";
    public const string PurchaseServicesAbroadIntegration = "TD17";
    public const string PurchaseIntraEuGoodsIntegration = "TD18";
    public const string PurchaseGoodsArt17C2Integration = "TD19";
    public const string SelfInvoiceRegularization = "TD20";
    public const string SelfInvoiceSplafoning = "TD21";
    public const string VatWarehouseExtraction = "TD22";
    public const string VatWarehouseExtractionWithPayment = "TD23";
    public const string DeferredInvoiceGoods = "TD24";
    public const string DeferredInvoiceTriangular = "TD25";
    public const string SaleOfDepreciableAssets = "TD26";
    public const string SelfInvoiceForSelfConsumption = "TD27";
    public const string PurchaseFromSanMarinoWithVat = "TD28";
    public const string OmittedOrIrregularInvoicingCommunication = "TD29";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [Invoice] = new(Invoice, "Invoice", "Invoice", "Fattura"),
        [AdvanceOnInvoice] = new(AdvanceOnInvoice, "AdvanceOnInvoice", "Advance / down payment on invoice", "Acconto / anticipo su fattura"),
        [AdvanceOnFee] = new(AdvanceOnFee, "AdvanceOnFee", "Advance / down payment on fee", "Acconto / anticipo su parcella"),
        [CreditNote] = new(CreditNote, "CreditNote", "Credit note", "Nota di credito"),
        [DebitNote] = new(DebitNote, "DebitNote", "Debit note", "Nota di debito"),
        [Fee] = new(Fee, "Fee", "Fee / professional parcel", "Parcella"),
        [SimplifiedInvoice] = new(SimplifiedInvoice, "SimplifiedInvoice", "Simplified invoice", "Fattura semplificata"),
        [SimplifiedCreditNote] = new(SimplifiedCreditNote, "SimplifiedCreditNote", "Simplified credit note", "Nota di credito semplificata"),
        [SimplifiedDebitNote] = new(SimplifiedDebitNote, "SimplifiedDebitNote", "Simplified debit note", "Nota di debito semplificata"),
        [InternalReverseChargeIntegration] = new(InternalReverseChargeIntegration, "InternalReverseChargeIntegration", "Reverse charge internal invoice integration", "Integrazione fattura reverse charge interno"),
        [PurchaseServicesAbroadIntegration] = new(PurchaseServicesAbroadIntegration, "PurchaseServicesAbroadIntegration", "Integration / self invoicing for purchase of services from abroad", "Integrazione / autofattura acquisto servizi dall'estero"),
        [PurchaseIntraEuGoodsIntegration] = new(PurchaseIntraEuGoodsIntegration, "PurchaseIntraEuGoodsIntegration", "Integration for purchase of intra-EU goods", "Integrazione per acquisto di beni intracomunitari"),
        [PurchaseGoodsArt17C2Integration] = new(PurchaseGoodsArt17C2Integration, "PurchaseGoodsArt17C2Integration", "Integration / self invoicing for purchase of goods ex art.17 c.2 DPR 633/72", "Integrazione / autofattura acquisto beni ex art.17 c.2 DPR 633/72"),
        [SelfInvoiceRegularization] = new(SelfInvoiceRegularization, "SelfInvoiceRegularization", "Self invoice for regularization and integration of invoices", "Autofattura per regolarizzazione e integrazione delle fatture"),
        [SelfInvoiceSplafoning] = new(SelfInvoiceSplafoning, "SelfInvoiceSplafoning", "Self invoice for splafoning", "Autofattura per splafonamento"),
        [VatWarehouseExtraction] = new(VatWarehouseExtraction, "VatWarehouseExtraction", "Extraction of goods from VAT Warehouse", "Estrazione beni da Deposito IVA"),
        [VatWarehouseExtractionWithPayment] = new(VatWarehouseExtractionWithPayment, "VatWarehouseExtractionWithPayment", "Extraction of goods from VAT Warehouse with payment of VAT", "Estrazione beni da Deposito IVA con versamento dell'IVA"),
        [DeferredInvoiceGoods] = new(DeferredInvoiceGoods, "DeferredInvoiceGoods", "Deferred invoice ex art.21, c.4, lett. a DPR 633/72", "Fattura differita art.21, c.4, terzo periodo lett. a)"),
        [DeferredInvoiceTriangular] = new(DeferredInvoiceTriangular, "DeferredInvoiceTriangular", "Deferred invoice ex art.21, c.4, lett. b DPR 633/72", "Fattura differita art.21, c.4, terzo periodo lett. b)"),
        [SaleOfDepreciableAssets] = new(SaleOfDepreciableAssets, "SaleOfDepreciableAssets", "Sale of depreciable assets and internal transfers (ex art.36)", "Cessione di beni ammortizzabili e passaggi interni"),
        [SelfInvoiceForSelfConsumption] = new(SelfInvoiceForSelfConsumption, "SelfInvoiceForSelfConsumption", "Self invoice for self-consumption or free transfer without recourse", "Fattura per autoconsumo o cessioni gratuite senza rivalsa"),
        [PurchaseFromSanMarinoWithVat] = new(PurchaseFromSanMarinoWithVat, "PurchaseFromSanMarinoWithVat", "Purchases from San Marino with VAT (paper invoice)", "Acquisti da San Marino con IVA (fattura cartacea)"),
        [OmittedOrIrregularInvoicingCommunication] = new(OmittedOrIrregularInvoicingCommunication, "OmittedOrIrregularInvoicingCommunication", "Communication for omitted or irregular invoicing (art. 6 c. 8 D.Lgs 471/97)", "Comunicazione per omessa o irregolare fatturazione")
    };

    public static IReadOnlyCollection<LookupCode> All => Items.Values.ToList().AsReadOnly();

    public static bool IsValid(string? code) => code != null && Items.ContainsKey(code);

    public static bool IsSimplified(string? code) => code is SimplifiedInvoice or SimplifiedCreditNote or SimplifiedDebitNote;

    public static bool IsOrdinary(string? code) => IsValid(code) && !IsSimplified(code);

    public static LookupCode? TryGet(string? code)
    {
        if (code != null && Items.TryGetValue(code, out var item))
            return item;
        return null;
    }
}
