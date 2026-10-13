using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Common;

/// <summary>
/// Contact channels (<Contatti> / <ContattiTrasmittente>).
/// </summary>
public class ContactInfo
{
	[XmlElement("Telefono")]
	public string? Phone { get; set; }

	[XmlElement("Fax")]
	public string? Fax { get; set; }

	[XmlElement("Email")]
	public string? Email { get; set; }
}
