using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

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
