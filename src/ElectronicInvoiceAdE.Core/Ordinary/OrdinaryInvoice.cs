using System.Collections.Generic;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;
using ElectronicInvoiceAdE.Lookups;
using ElectronicInvoiceAdE.Ordinary.Bodies;
using ElectronicInvoiceAdE.Ordinary.Headers;

namespace ElectronicInvoiceAdE.Ordinary;

/// <summary>
/// Represents an Italian Ordinary Electronic Invoice (<p:FatturaElettronica> with format FPA12 or FPR12).
/// Complies with Agenzia delle Entrate Technical Specifications v1.9.1.
/// </summary>
[XmlRoot("FatturaElettronica", Namespace = XmlNamespaces.FatturaOrdinariaV12)]
public class OrdinaryInvoice
{
    public OrdinaryInvoice()
    {
        Header = new();
        Bodies = new();
    }

    [XmlAttribute("versione")]
    public string Version { get; set; } = TransmissionFormat.PrivateParties;

    [XmlAttribute("SistemaEmittente")]
    public string? IssuingSystem { get; set; }

    [XmlElement("FatturaElettronicaHeader")]
    public InvoiceHeader Header { get; set; }

    [XmlElement("FatturaElettronicaBody")]
    public List<InvoiceBody> Bodies { get; set; }

    /// <summary>
    /// Gets the primary body (the first body in Bodies list, or creates one if empty).
    /// Most electronic invoices contain exactly one body.
    /// </summary>
    [XmlIgnore]
    public InvoiceBody PrimaryBody
    {
        get
        {
            if (Bodies.Count == 0)
            {
                Bodies.Add(new InvoiceBody());
            }
            return Bodies[0];
        }
    }

    /// <summary>
    /// Factory helper to create an instance pre-configured for B2B / B2C (FPR12) or Public Administration (FPA12).
    /// </summary>
    public static OrdinaryInvoice Create(string format = TransmissionFormat.PrivateParties)
    {
        var invoice = new OrdinaryInvoice
        {
            Version = format
        };
        invoice.Header.TransmissionData.Format = format;
        invoice.Bodies.Add(new InvoiceBody());
        return invoice;
    }
}
