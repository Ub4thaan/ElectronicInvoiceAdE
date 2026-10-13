using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// Tax representative in a simplified invoice.
/// </summary>
public class SimplifiedTaxRepresentative
{
	[XmlElement("IdFiscaleIVA")]
	public TaxId TaxId { get; set; } = new();

	[XmlElement("Denominazione")]
	public string? CompanyName { get; set; }

	[XmlElement("Nome")]
	public string? FirstName { get; set; }

	[XmlElement("Cognome")]
	public string? LastName { get; set; }
}
