using System.Xml.Serialization;
using ElectronicInvoiceAdE.Lookups;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Withholding tax data (<DatiRitenuta>).
/// </summary>
public class WithholdingTax
{
	[XmlElement("TipoRitenuta")]
	public string Type { get; set; } = WithholdingType.NaturalPersons;

	[XmlElement("ImportoRitenuta")]
	public decimal Amount { get; set; }

	[XmlElement("AliquotaRitenuta")]
	public decimal Rate { get; set; }

	[XmlElement("CausalePagamento")]
	public string PaymentReason { get; set; } = "A";
}
