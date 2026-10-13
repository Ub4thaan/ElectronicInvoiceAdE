using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;


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
