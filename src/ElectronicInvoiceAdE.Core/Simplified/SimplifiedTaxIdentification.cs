using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// Fiscal identification in a simplified invoice.
/// </summary>
public class SimplifiedTaxIdentification
{
	[XmlElement("IdFiscaleIVA")]
	public TaxId? TaxId { get; set; }

	[XmlElement("CodiceFiscale")]
	public string? FiscalCode { get; set; }
}
