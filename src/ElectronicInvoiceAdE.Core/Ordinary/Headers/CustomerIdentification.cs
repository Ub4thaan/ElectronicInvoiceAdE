using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Headers;

/// <summary>
/// Identification data of the customer/buyer (<DatiAnagrafici> in <CessionarioCommittente>).
/// </summary>
public class CustomerIdentification
{
	[XmlElement("IdFiscaleIVA")]
	public TaxId? TaxId { get; set; }

	[XmlElement("CodiceFiscale")]
	public string? FiscalCode { get; set; }

	[XmlElement("Anagrafica")]
	public LegalName LegalName { get; set; } = new();
}
