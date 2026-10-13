using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// Customer identification in a simplified invoice (&lt;CessionarioCommittente&gt;).
/// </summary>
public class SimplifiedCustomer
{
	[XmlElement("IdentificativiFiscali")]
	public SimplifiedTaxIdentification? TaxIdentification { get; set; }

	[XmlElement("AltriDatiIdentificativi")]
	public SimplifiedOtherIdentification? OtherIdentification { get; set; }
}
