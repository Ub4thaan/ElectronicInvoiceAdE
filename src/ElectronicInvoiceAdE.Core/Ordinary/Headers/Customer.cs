using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Headers;

/// <summary>
/// Identification data of the customer/buyer (<DatiAnagrafici> in <CessionarioCommittente>).
/// </summary>
public class CustomerIdentification
{
    [XmlElement("IdFiscaleIVA")]
    public TaxId? TaxId { get; set; }

    [XmlElement("CodiceFiscale")]
    public string? FiscalCode { get; set; }

    [XmlElement("Anagrafica")]
    public LegalName LegalName { get; set; } = new();
}

/// <summary>
/// Buyer / Customer data (<CessionarioCommittente>).
/// </summary>
public class Customer
{
    [XmlElement("DatiAnagrafici")]
    public CustomerIdentification Identification { get; set; } = new();

    [XmlElement("Sede")]
    public Address Address { get; set; } = new();

    [XmlElement("StabileOrganizzazione")]
    public Address? PermanentEstablishment { get; set; }

    [XmlElement("RappresentanteFiscale")]
    public TaxRepresentative? Representative { get; set; }
}
