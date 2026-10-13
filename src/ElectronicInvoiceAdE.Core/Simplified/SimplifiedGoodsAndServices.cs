using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// Line item for goods or services in a simplified invoice (&lt;DatiBeniServizi&gt;).
/// </summary>
public class SimplifiedGoodsAndServices
{
	[XmlElement("Descrizione")]
	public string Description { get; set; } = string.Empty;

	[XmlElement("Importo")]
	public decimal Amount { get; set; }

	[XmlElement("DatiIVA")]
	public SimplifiedVatData? VatData { get; set; }

	[XmlElement("Natura")]
	public string? VatNature { get; set; }

	[XmlElement("RiferimentoNormativo")]
	public string? RegulatoryReference { get; set; }
}
