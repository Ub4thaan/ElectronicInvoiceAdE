using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Common;

/// <summary>
/// Base class for procurement and document references (<DatiDocumento>).
/// Used by Purchase Orders, Contracts, Agreements, Receptions, Related Invoices.
/// </summary>
public class DocumentReference
{
	[XmlElement("RiferimentoNumeroLinea")]
	public List<int> LineNumbers { get; set; } = new();

	[XmlElement("IdDocumento")]
	public string DocumentId { get; set; } = string.Empty;

	[XmlElement("Data")]
	public DateTime? Date { get; set; }

	[XmlElement("NumItem")]
	public string? ItemNumber { get; set; }

	[XmlElement("CodiceCommessaConvenzione")]
	public string? AgreementCode { get; set; }

	[XmlElement("CodiceCUP")]
	public string? CupCode { get; set; }

	[XmlElement("CodiceCIG")]
	public string? CigCode { get; set; }
}
