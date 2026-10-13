using System.Xml.Serialization;
using ElectronicInvoiceAdE.Lookups;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Pension and social security fund contributions (<DatiCassaPrevidenziale>).
/// </summary>
public class PensionFundContribution
{
	[XmlElement("TipoCassa")]
	public string FundType { get; set; } = PensionFundType.Inps;

	[XmlElement("AlCassa")]
	public decimal ContributionRate { get; set; }

	[XmlElement("ImportoContributoCassa")]
	public decimal ContributionAmount { get; set; }

	[XmlElement("ImponibileCassa")]
	public decimal? TaxableAmount { get; set; }

	[XmlElement("AliquotaIVA")]
	public decimal VatRate { get; set; }

	[XmlElement("Ritenuta")]
	public string? WithholdingTax { get; set; }

	[XmlElement("Natura")]
	public string? VatNature { get; set; }

	[XmlElement("RiferimentoAmministrazione")]
	public string? SubjectReference { get; set; }
}
