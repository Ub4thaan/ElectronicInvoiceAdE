using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// VAT information on a simplified invoice line (&lt;DatiIVA&gt;).
/// </summary>
public class SimplifiedVatData
{
	[XmlElement("Imposta")]
	public decimal? TaxAmount { get; set; }

	[XmlElement("Aliquota")]
	public decimal? Rate { get; set; }
}
