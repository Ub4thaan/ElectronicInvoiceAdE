using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Carrier identification (<DatiAnagraficiVettore>).
/// </summary>
public class CarrierIdentification
{
	[XmlElement("IdFiscaleIVA")]
	public TaxId TaxId { get; set; } = new();

	[XmlElement("CodiceFiscale")]
	public string? FiscalCode { get; set; }

	[XmlElement("Anagrafica")]
	public LegalName LegalName { get; set; } = new();

	[XmlElement("NumeroLicenzaGuida")]
	public string? DrivingLicenseNumber { get; set; }
}
