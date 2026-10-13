using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Headers;


/// <summary>
/// Root header for Ordinary Invoices (<FatturaElettronicaHeader>).
/// </summary>
public class InvoiceHeader
{
	[XmlElement("DatiTrasmissione")]
	public TransmissionData TransmissionData { get; set; } = new();

	[XmlElement("CedentePrestatore")]
	public Supplier Supplier { get; set; } = new();

	[XmlElement("RappresentanteFiscale")]
	public TaxRepresentative? TaxRepresentative { get; set; }

	[XmlElement("CessionarioCommittente")]
	public Customer Customer { get; set; } = new();

	[XmlElement("TerzoIntermediarioOSoggettoEmittente")]
	public ThirdPartyIntermediary? ThirdPartyIntermediary { get; set; }

	[XmlElement("SoggettoEmittente")]
	public string? IssuingSubject { get; set; }
}
