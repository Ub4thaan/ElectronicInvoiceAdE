using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Product / Item identification code (<CodiceArticolo>).
/// </summary>
public class ItemCode
{
	[XmlElement("CodiceTipo")]
	public string CodeType { get; set; } = string.Empty;

	[XmlElement("CodiceValore")]
	public string CodeValue { get; set; } = string.Empty;
}
