using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Headers;

/// <summary>
/// Identification data for third party intermediary or issuing subject.
/// </summary>
public class ThirdPartyIntermediaryIdentification
{
	[XmlElement("IdFiscaleIVA")]
	public TaxId? TaxId { get; set; }

	[XmlElement("CodiceFiscale")]
	public string? FiscalCode { get; set; }

	[XmlElement("Anagrafica")]
	public LegalName LegalName { get; set; } = new();
}
