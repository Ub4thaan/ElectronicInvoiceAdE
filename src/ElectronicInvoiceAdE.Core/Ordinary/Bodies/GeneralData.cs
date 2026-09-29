using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;
using ElectronicInvoiceAdE.Lookups;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Withholding tax data (<DatiRitenuta>).
/// </summary>
public class WithholdingTax
{
    [XmlElement("TipoRitenuta")]
    public string Type { get; set; } = WithholdingType.NaturalPersons;

    [XmlElement("ImportoRitenuta")]
    public decimal Amount { get; set; }

    [XmlElement("AliquotaRitenuta")]
    public decimal Rate { get; set; }

    [XmlElement("CausalePagamento")]
    public string PaymentReason { get; set; } = "A";
}

/// <summary>
/// Virtual stamp duty data (<DatiBollo>).
/// </summary>
public class StampDuty
{
    [XmlElement("BolloVirtuale")]
    public string VirtualStamp { get; set; } = "SI";

    [XmlElement("ImportoBollo")]
    public decimal? Amount { get; set; }
}

/// <summary>
/// Pension and social security fund contributions (<DatiCassaPrevidenziale>).
/// </summary>
public class PensionFundContribution
{
    [XmlElement("TipoCassa")]
    public string FundType { get; set; } = PensionFundType.Inps;

    [XmlElement("AlCassa")]
    public decimal ContributionRate { get; set; }

    [XmlElement("ImportoContributoCassa")]
    public decimal ContributionAmount { get; set; }

    [XmlElement("ImponibileCassa")]
    public decimal? TaxableAmount { get; set; }

    [XmlElement("AliquotaIVA")]
    public decimal VatRate { get; set; }

    [XmlElement("Ritenuta")]
    public string? WithholdingTax { get; set; }

    [XmlElement("Natura")]
    public string? VatNature { get; set; }

    [XmlElement("RiferimentoAmministrazione")]
    public string? SubjectReference { get; set; }
}

/// <summary>
/// Delivery note (DDT) reference (<DatiDDT>).
/// </summary>
public class DeliveryNoteData
{
    [XmlElement("NumeroDDT")]
    public string DocumentNumber { get; set; } = string.Empty;

    [XmlElement("DataDDT")]
    public DateTime Date { get; set; }

    [XmlElement("RiferimentoNumeroLinea")]
    public List<int> LineNumbers { get; set; } = new();
}

/// <summary>
/// Main invoice reference for invoices related to a parent invoice (<DatiFatturaPrincipale>).
/// </summary>
public class MainInvoiceData
{
    [XmlElement("NumeroFatturaPrincipale")]
    public string InvoiceNumber { get; set; } = string.Empty;

    [XmlElement("DataFatturaPrincipale")]
    public DateTime InvoiceDate { get; set; }
}

/// <summary>
/// Carrier identification (<DatiAnagraficiVettore>).
/// </summary>
public class CarrierIdentification
{
    [XmlElement("IdFiscaleIVA")]
    public TaxId TaxId { get; set; } = new();

    [XmlElement("CodiceFiscale")]
    public string? FiscalCode { get; set; }

    [XmlElement("Anagrafica")]
    public LegalName LegalName { get; set; } = new();

    [XmlElement("NumeroLicenzaGuida")]
    public string? DrivingLicenseNumber { get; set; }
}

/// <summary>
/// Transport and shipping metadata (<DatiTrasporto>).
/// </summary>
public class TransportData
{
    [XmlElement("DatiAnagraficiVettore")]
    public CarrierIdentification? Carrier { get; set; }

    [XmlElement("MezzoTrasporto")]
    public string? TransportMean { get; set; }

    [XmlElement("CausaleTrasporto")]
    public string? TransportCausale { get; set; }

    [XmlElement("NumeroColli")]
    public int? PackagesNumber { get; set; }

    [XmlElement("Descrizione")]
    public string? GoodsDescription { get; set; }

    [XmlElement("PesoLordo")]
    public decimal? TotalWeightKg { get; set; }

    [XmlElement("PesoNetto")]
    public decimal? NetWeightKg { get; set; }

    [XmlElement("DataOraRitiro")]
    public DateTime? PickupDateTime { get; set; }

    [XmlElement("DataInizioTrasporto")]
    public DateTime? TransportStartDateTime { get; set; }

    [XmlElement("TipoResa")]
    public string? Incoterms { get; set; }

    [XmlElement("IndirizzoResa")]
    public Address? DeliveryAddress { get; set; }

    [XmlElement("DataOraConsegna")]
    public DateTime? DeliveryDateTime { get; set; }
}

/// <summary>
/// General document metadata (<DatiGeneraliDocumento>).
/// </summary>
public class GeneralDocumentData
{
    [XmlElement("TipoDocumento")]
    public string DocumentType { get; set; } = Lookups.DocumentType.Invoice;

    [XmlElement("Divisa")]
    public string Currency { get; set; } = "EUR";

    [XmlElement("Data")]
    public DateTime Date { get; set; }

    [XmlElement("Numero")]
    public string Number { get; set; } = string.Empty;

    [XmlElement("DatiRitenuta")]
    public WithholdingTax? WithholdingTax { get; set; }

    [XmlElement("DatiBollo")]
    public StampDuty? StampDuty { get; set; }

    [XmlElement("DatiCassaPrevidenziale")]
    public List<PensionFundContribution> PensionFunds { get; set; } = new();

    [XmlElement("ScontoMaggiorazione")]
    public List<DiscountOrSurcharge> DiscountsOrSurcharges { get; set; } = new();

    [XmlElement("ImportoTotaleDocumento")]
    public decimal? TotalAmount { get; set; }

    [XmlElement("Arrotondamento")]
    public decimal? Rounding { get; set; }

    [XmlElement("Causale")]
    public List<string> Descriptions { get; set; } = new();

    [XmlElement("Art73")]
    public string? Art73 { get; set; }
}

/// <summary>
/// General section of an invoice body (<DatiGenerali>).
/// </summary>
public class GeneralData
{
    [XmlElement("DatiGeneraliDocumento")]
    public GeneralDocumentData DocumentGeneralData { get; set; } = new();

    [XmlElement("DatiOrdineAcquisto")]
    public List<DocumentReference> PurchaseOrders { get; set; } = new();

    [XmlElement("DatiContratto")]
    public List<DocumentReference> Contracts { get; set; } = new();

    [XmlElement("DatiConvenzione")]
    public List<DocumentReference> Agreements { get; set; } = new();

    [XmlElement("DatiRicezione")]
    public List<DocumentReference> ReceptionData { get; set; } = new();

    [XmlElement("DatiFattureCollegate")]
    public List<DocumentReference> RelatedInvoices { get; set; } = new();

    [XmlElement("DatiDDT")]
    public List<DeliveryNoteData> DeliveryNotes { get; set; } = new();

    [XmlElement("DatiTrasporto")]
    public TransportData? TransportData { get; set; }

    [XmlElement("DatiFatturaPrincipale")]
    public MainInvoiceData? MainInvoice { get; set; }
}
