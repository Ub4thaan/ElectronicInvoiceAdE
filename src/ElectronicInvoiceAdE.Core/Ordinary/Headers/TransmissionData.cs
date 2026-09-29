using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;
using ElectronicInvoiceAdE.Lookups;

namespace ElectronicInvoiceAdE.Ordinary.Headers;

/// <summary>
/// Transmission metadata (<DatiTrasmissione>) identifying transmitter, recipient, format, and progressivo.
/// </summary>
public class TransmissionData
{
    [XmlElement("IdTrasmittente")]
    public TaxId TransmitterId { get; set; } = new();

    [XmlElement("ProgressivoInvio")]
    public string TransmissionSequence { get; set; } = string.Empty;

    [XmlElement("FormatoTrasmissione")]
    public string Format { get; set; } = TransmissionFormat.PrivateParties;

    [XmlElement("CodiceDestinatario")]
    public string RecipientCode { get; set; } = "0000000";

    [XmlElement("ContattiTrasmittente")]
    public ContactInfo? TransmitterContacts { get; set; }

    [XmlElement("PECDestinatario")]
    public string? RecipientPec { get; set; }
}
