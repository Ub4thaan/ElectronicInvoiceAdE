using System;
using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// Reference to an amended/rectified invoice in a simplified credit/debit note (&lt;DatiFatturaRettificata&gt;).
/// </summary>
public class RectifiedInvoiceData
{
	[XmlElement("NumeroFattura")]
	public string InvoiceNumber { get; set; } = string.Empty;

	[XmlElement("DataFattura")]
	public DateTime InvoiceDate { get; set; }

	[XmlElement("ElementiRettificati")]
	public string? RectifiedElements { get; set; }
}
