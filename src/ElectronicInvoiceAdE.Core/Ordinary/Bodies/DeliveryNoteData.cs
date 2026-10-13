using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Delivery note (DDT) reference (<DatiDDT>).
/// </summary>
public class DeliveryNoteData
{
	[XmlElement("NumeroDDT")]
	public string DocumentNumber { get; set; } = string.Empty;

	[XmlElement("DataDDT")]
	public DateTime Date { get; set; }

	[XmlElement("RiferimentoNumeroLinea")]
	public List<int> LineNumbers { get; set; } = new();
}
