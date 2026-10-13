using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// Other identification data for customer in simplified invoice.
/// </summary>
public class SimplifiedOtherIdentification
{
	[XmlElement("Denominazione")]
	public string? CompanyName { get; set; }

	[XmlElement("Nome")]
	public string? FirstName { get; set; }

	[XmlElement("Cognome")]
	public string? LastName { get; set; }

	[XmlElement("Sede")]
	public Address? Address { get; set; }

	[XmlElement("StabileOrganizzazione")]
	public Address? PermanentEstablishment { get; set; }

	[XmlElement("RappresentanteFiscale")]
	public SimplifiedTaxRepresentative? Representative { get; set; }
}
