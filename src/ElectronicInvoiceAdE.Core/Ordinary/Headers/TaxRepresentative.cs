using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Headers;

/// <summary>
/// Tax representative (<RappresentanteFiscale>).
/// </summary>
public class TaxRepresentative
{
	[XmlElement("DatiAnagrafici")]
	public TaxRepresentativeIdentification Identification { get; set; } = new();

	[XmlElement("Sede")]
	public Address? Address { get; set; }
}
