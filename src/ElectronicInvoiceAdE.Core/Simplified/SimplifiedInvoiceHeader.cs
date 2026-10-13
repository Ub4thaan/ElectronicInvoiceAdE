using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// Header for simplified invoice (&lt;FatturaElettronicaHeader&gt;).
/// </summary>
public class SimplifiedInvoiceHeader
{
	[XmlElement("DatiTrasmissione")]
	public Ordinary.Headers.TransmissionData TransmissionData { get; set; } = new();

	[XmlElement("CedentePrestatore")]
	public SimplifiedSupplier Supplier { get; set; } = new();

	[XmlElement("CessionarioCommittente")]
	public SimplifiedCustomer Customer { get; set; } = new();
}
