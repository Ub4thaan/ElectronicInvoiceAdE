using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Product / Item identification code (<CodiceArticolo>).
/// </summary>
public class ItemCode
{
    [XmlElement("CodiceTipo")]
    public string CodeType { get; set; } = string.Empty;

    [XmlElement("CodiceValore")]
    public string CodeValue { get; set; } = string.Empty;
}

/// <summary>
/// Detail of an invoice line item (<DettaglioLinee>).
/// </summary>
public class InvoiceLine
{
    [XmlElement("NumeroLinea")]
    public int LineNumber { get; set; }

    [XmlElement("TipoCessionePrestazione")]
    public string? SupplyType { get; set; }

    [XmlElement("CodiceArticolo")]
    public List<ItemCode> ItemCodes { get; set; } = new();

    [XmlElement("Descrizione")]
    public string Description { get; set; } = string.Empty;

    [XmlElement("Quantita")]
    public decimal? Quantity { get; set; }

    [XmlElement("UnitaMisura")]
    public string? UnitOfMeasure { get; set; }

    [XmlElement("DataInizioPeriodo")]
    public DateTime? StartDate { get; set; }

    [XmlElement("DataFinePeriodo")]
    public DateTime? EndDate { get; set; }

    [XmlElement("PrezzoUnitario")]
    public decimal UnitPrice { get; set; }

    [XmlElement("ScontoMaggiorazione")]
    public List<DiscountOrSurcharge> DiscountsOrSurcharges { get; set; } = new();

    [XmlElement("PrezzoTotale")]
    public decimal TotalPrice { get; set; }

    [XmlElement("AliquotaIVA")]
    public decimal VatRate { get; set; }

    [XmlElement("Ritenuta")]
    public string? WithholdingTax { get; set; }

    [XmlElement("Natura")]
    public string? VatNature { get; set; }

    [XmlElement("RiferimentoAmministrazione")]
    public string? AdministrativeReference { get; set; }

    [XmlElement("AltriDatiGestionali")]
    public List<OtherManagementData> OtherManagementData { get; set; } = new();
}

/// <summary>
/// VAT breakdown and summary data (<DatiRiepilogo>).
/// </summary>
public class VatSummary
{
    [XmlElement("AliquotaIVA")]
    public decimal VatRate { get; set; }

    [XmlElement("Natura")]
    public string? VatNature { get; set; }

    [XmlElement("SpeseAccessorie")]
    public decimal? Expenses { get; set; }

    [XmlElement("Arrotondamento")]
    public decimal? Rounding { get; set; }

    [XmlElement("ImponibileImporto")]
    public decimal TaxableAmount { get; set; }

    [XmlElement("Imposta")]
    public decimal TaxAmount { get; set; }

    [XmlElement("EsigibilitaIVA")]
    public string? VatCollectibility { get; set; }

    [XmlElement("RiferimentoNormativo")]
    public string? RegulatoryReference { get; set; }
}

/// <summary>
/// Goods and services block (<DatiBeniServizi>).
/// </summary>
public class GoodsAndServices
{
    [XmlElement("DettaglioLinee")]
    public List<InvoiceLine> Lines { get; set; } = new();

    [XmlElement("DatiRiepilogo")]
    public List<VatSummary> VatSummaries { get; set; } = new();
}
