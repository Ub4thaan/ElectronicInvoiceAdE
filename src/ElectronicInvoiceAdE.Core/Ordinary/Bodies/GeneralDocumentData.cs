using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;
using ElectronicInvoiceAdE.Lookups;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

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
