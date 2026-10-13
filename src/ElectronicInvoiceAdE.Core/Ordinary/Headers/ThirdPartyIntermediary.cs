using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Ordinary.Headers;

/// <summary>
/// Third party intermediary or issuing subject (<TerzoIntermediarioOSoggettoEmittente>).
/// </summary>
public class ThirdPartyIntermediary
{
	[XmlElement("DatiAnagrafici")]
	public ThirdPartyIntermediaryIdentification Identification { get; set; } = new();
}
