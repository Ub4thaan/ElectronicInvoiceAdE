using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Attached document embedded inside the invoice XML (<Allegati>).
/// </summary>
public class Attachment
{
	[XmlElement("NomeAttachment")]
	public string AttachmentName { get; set; } = string.Empty;

	[XmlElement("AlgoritmoCompressione")]
	public string? CompressionAlgorithm { get; set; }

	[XmlElement("FormatoAttachment")]
	public string? Format { get; set; }

	[XmlElement("DescrizioneAttachment")]
	public string? Description { get; set; }

	[XmlElement("Attachment")]
	public byte[]? Data { get; set; }
}
