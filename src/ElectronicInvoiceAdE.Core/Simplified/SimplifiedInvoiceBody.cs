using System.Collections.Generic;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Ordinary.Bodies;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// Body of a simplified invoice (&lt;FatturaElettronicaBody&gt;).
/// </summary>
public class SimplifiedInvoiceBody
{
	[XmlElement("DatiGenerali")]
	public SimplifiedGeneralData GeneralData { get; set; } = new();

	[XmlElement("DatiBeniServizi")]
	public List<SimplifiedGoodsAndServices> GoodsAndServices { get; set; } = new();

	[XmlElement("Allegati")]
	public List<Attachment> Attachments { get; set; } = new();
}
