using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Common;

/// <summary>
/// Discount or surcharge applied (<ScontoMaggiorazione>).
/// </summary>
public class DiscountOrSurcharge
{
	[XmlElement("Tipo")]
	public string Type { get; set; } = Lookups.DiscountOrSurchargeType.Discount;

	[XmlElement("Percentuale")]
	public decimal? Percentage { get; set; }

	[XmlElement("Importo")]
	public decimal? Amount { get; set; }
}
