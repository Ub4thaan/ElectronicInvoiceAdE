using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// Supplier data in a simplified invoice (&lt;CedentePrestatore&gt;).
/// </summary>
public class SimplifiedSupplier
{
	[XmlElement("IdFiscaleIVA")]
	public TaxId TaxId { get; set; } = new();

	[XmlElement("CodiceFiscale")]
	public string? FiscalCode { get; set; }

	[XmlElement("Denominazione")]
	public string? CompanyName { get; set; }

	[XmlElement("Nome")]
	public string? FirstName { get; set; }

	[XmlElement("Cognome")]
	public string? LastName { get; set; }

	[XmlElement("Sede")]
	public Address Address { get; set; } = new();

	[XmlElement("StabileOrganizzazione")]
	public Address? PermanentEstablishment { get; set; }

	[XmlElement("RappresentanteFiscale")]
	public SimplifiedTaxRepresentative? Representative { get; set; }

	[XmlElement("IscrizioneREA")]
	public ReaRegistration? ReaRegistration { get; set; }

	[XmlElement("RegimeFiscale")]
	public string TaxRegime { get; set; } = Lookups.TaxRegime.Ordinary;
}
