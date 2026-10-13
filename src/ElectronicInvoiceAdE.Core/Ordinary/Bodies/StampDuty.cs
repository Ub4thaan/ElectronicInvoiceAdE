using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Virtual stamp duty data (<DatiBollo>).
/// </summary>
public class StampDuty
{
	[XmlElement("BolloVirtuale")]
	public string VirtualStamp { get; set; } = "SI";

	[XmlElement("ImportoBollo")]
	public decimal? Amount { get; set; }
}
