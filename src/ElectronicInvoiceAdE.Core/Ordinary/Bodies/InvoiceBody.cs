using System.Collections.Generic;
using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Root body element for Ordinary Invoices (<FatturaElettronicaBody>).
/// </summary>
public class InvoiceBody
{
    [XmlElement("DatiGenerali")]
    public GeneralData GeneralData { get; set; } = new();

    [XmlElement("DatiBeniServizi")]
    public GoodsAndServices GoodsAndServices { get; set; } = new();

    [XmlElement("DatiVeicoli")]
    public VehicleData? VehicleData { get; set; }

    [XmlElement("DatiPagamento")]
    public List<PaymentData> Payments { get; set; } = new();

    [XmlElement("Allegati")]
    public List<Attachment> Attachments { get; set; } = new();
}
