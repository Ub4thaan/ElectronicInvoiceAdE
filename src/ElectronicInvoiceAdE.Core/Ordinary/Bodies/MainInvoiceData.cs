using System;
using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Main invoice reference for invoices related to a parent invoice (<DatiFatturaPrincipale>).
/// </summary>
public class MainInvoiceData
{
	[XmlElement("NumeroFatturaPrincipale")]
	public string InvoiceNumber { get; set; } = string.Empty;

	[XmlElement("DataFatturaPrincipale")]
	public DateTime InvoiceDate { get; set; }
}
