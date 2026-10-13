using System.Collections.Generic;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// General section of an invoice body (<DatiGenerali>).
/// </summary>
public class GeneralData
{
	[XmlElement("DatiGeneraliDocumento")]
	public GeneralDocumentData DocumentGeneralData { get; set; } = new();

	[XmlElement("DatiOrdineAcquisto")]
	public List<DocumentReference> PurchaseOrders { get; set; } = new();

	[XmlElement("DatiContratto")]
	public List<DocumentReference> Contracts { get; set; } = new();

	[XmlElement("DatiConvenzione")]
	public List<DocumentReference> Agreements { get; set; } = new();

	[XmlElement("DatiRicezione")]
	public List<DocumentReference> ReceptionData { get; set; } = new();

	[XmlElement("DatiFattureCollegate")]
	public List<DocumentReference> RelatedInvoices { get; set; } = new();

	[XmlElement("DatiDDT")]
	public List<DeliveryNoteData> DeliveryNotes { get; set; } = new();

	[XmlElement("DatiTrasporto")]
	public TransportData? TransportData { get; set; }

	[XmlElement("DatiFatturaPrincipale")]
	public MainInvoiceData? MainInvoice { get; set; }
}
