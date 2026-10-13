using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Headers;

/// <summary>
/// Identification data for tax representative.
/// </summary>
public class TaxRepresentativeIdentification
{
	[XmlElement("IdFiscaleIVA")]
	public TaxId TaxId { get; set; } = new();

	[XmlElement("CodiceFiscale")]
	public string? FiscalCode { get; set; }

	[XmlElement("Anagrafica")]
	public LegalName LegalName { get; set; } = new();
}
