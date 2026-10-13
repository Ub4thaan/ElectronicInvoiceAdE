using System;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Transport and shipping metadata (<DatiTrasporto>).
/// </summary>
public class TransportData
{
	[XmlElement("DatiAnagraficiVettore")]
	public CarrierIdentification? Carrier { get; set; }

	[XmlElement("MezzoTrasporto")]
	public string? TransportMean { get; set; }

	[XmlElement("CausaleTrasporto")]
	public string? TransportCausale { get; set; }

	[XmlElement("NumeroColli")]
	public int? PackagesNumber { get; set; }

	[XmlElement("Descrizione")]
	public string? GoodsDescription { get; set; }

	[XmlElement("PesoLordo")]
	public decimal? TotalWeightKg { get; set; }

	[XmlElement("PesoNetto")]
	public decimal? NetWeightKg { get; set; }

	[XmlElement("DataOraRitiro")]
	public DateTime? PickupDateTime { get; set; }

	[XmlElement("DataInizioTrasporto")]
	public DateTime? TransportStartDateTime { get; set; }

	[XmlElement("TipoResa")]
	public string? Incoterms { get; set; }

	[XmlElement("IndirizzoResa")]
	public Address? DeliveryAddress { get; set; }

	[XmlElement("DataOraConsegna")]
	public DateTime? DeliveryDateTime { get; set; }
}
