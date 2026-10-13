using System;
using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Common;

/// <summary>
/// Administrative / custom management details (<AltriDatiGestionali>).
/// Used for fuel card license plates, sports worker exemptions, intent declarations, etc.
/// </summary>
public class OtherManagementData
{
	[XmlElement("TipoDato")]
	public string DataType { get; set; } = string.Empty;

	[XmlElement("RiferimentoTesto")]
	public string? TextReference { get; set; }

	[XmlElement("RiferimentoNumero")]
	public decimal? NumberReference { get; set; }

	[XmlElement("RiferimentoData")]
	public DateTime? DateReference { get; set; }
}
