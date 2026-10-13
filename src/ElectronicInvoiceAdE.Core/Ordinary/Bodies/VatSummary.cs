using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// VAT breakdown and summary data (<DatiRiepilogo>).
/// </summary>
public class VatSummary
{
	[XmlElement("AliquotaIVA")]
	public decimal VatRate { get; set; }

	[XmlElement("Natura")]
	public string? VatNature { get; set; }

	[XmlElement("SpeseAccessorie")]
	public decimal? Expenses { get; set; }

	[XmlElement("Arrotondamento")]
	public decimal? Rounding { get; set; }

	[XmlElement("ImponibileImporto")]
	public decimal TaxableAmount { get; set; }

	[XmlElement("Imposta")]
	public decimal TaxAmount { get; set; }

	[XmlElement("EsigibilitaIVA")]
	public string? VatCollectibility { get; set; }

	[XmlElement("RiferimentoNormativo")]
	public string? RegulatoryReference { get; set; }
}
