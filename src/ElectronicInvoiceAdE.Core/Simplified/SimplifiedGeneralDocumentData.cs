using System;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Ordinary.Bodies;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// General document data for a simplified invoice (&lt;DatiGeneraliDocumento&gt;).
/// </summary>
public class SimplifiedGeneralDocumentData
{
	[XmlElement("TipoDocumento")]
	public string DocumentType { get; set; } = Lookups.DocumentType.SimplifiedInvoice;

	[XmlElement("Divisa")]
	public string Currency { get; set; } = "EUR";

	[XmlElement("Data")]
	public DateTime Date { get; set; }

	[XmlElement("Numero")]
	public string Number { get; set; } = string.Empty;

	[XmlElement("DatiBollo")]
	public StampDuty? StampDuty { get; set; }
}
