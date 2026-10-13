namespace ElectronicInvoiceAdE.Common;

/// <summary>
/// Official XML namespaces used by the Italian Exchange System (SDI).
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "S5332:Using http protocol is insecure. Use https instead.", Justification = "Standard and official XML namespace identifiers, not web connection URLs.")]
public static class XmlNamespaces
{
	public const string FatturaOrdinariaV12 = "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v1.2";
	public const string FatturaSemplificataV10 = "http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v1.0";
	public const string XmlDsig = "http://www.w3.org/2000/09/xmldsig#";
	public const string XmlSchemaInstance = "http://www.w3.org/2001/XMLSchema-instance";
}
