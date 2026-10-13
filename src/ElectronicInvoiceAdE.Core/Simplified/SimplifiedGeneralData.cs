using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// General data block for a simplified invoice (&lt;DatiGenerali&gt;).
/// </summary>
public class SimplifiedGeneralData
{
	[XmlElement("DatiGeneraliDocumento")]
	public SimplifiedGeneralDocumentData DocumentGeneralData { get; set; } = new();

	[XmlElement("DatiFatturaRettificata")]
	public RectifiedInvoiceData? RectifiedInvoice { get; set; }
}
