using System.Collections.Generic;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Lookups;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Payment information section (<DatiPagamento>).
/// </summary>
public class PaymentData
{
	[XmlElement("CondizioniPagamento")]
	public string PaymentTerms { get; set; } = Lookups.PaymentTerms.Full;

	[XmlElement("DettaglioPagamento")]
	public List<PaymentDetail> PaymentDetails { get; set; } = new();
}
