using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;

namespace ElectronicInvoiceAdE.Ordinary.Headers;

/// <summary>
/// Tax representative (<RappresentanteFiscale>).
/// </summary>
public class TaxRepresentative
{
    [XmlElement("DatiAnagrafici")]
    public TaxRepresentativeIdentification Identification { get; set; } = new();

    [XmlElement("Sede")]
    public Address? Address { get; set; }
}

public class TaxRepresentativeIdentification
{
    [XmlElement("IdFiscaleIVA")]
    public TaxId TaxId { get; set; } = new();

    [XmlElement("CodiceFiscale")]
    public string? FiscalCode { get; set; }

    [XmlElement("Anagrafica")]
    public LegalName LegalName { get; set; } = new();
}

/// <summary>
/// Third party intermediary or issuing subject (<TerzoIntermediarioOSoggettoEmittente>).
/// </summary>
public class ThirdPartyIntermediary
{
    [XmlElement("DatiAnagrafici")]
    public ThirdPartyIntermediaryIdentification Identification { get; set; } = new();
}

public class ThirdPartyIntermediaryIdentification
{
    [XmlElement("IdFiscaleIVA")]
    public TaxId? TaxId { get; set; }

    [XmlElement("CodiceFiscale")]
    public string? FiscalCode { get; set; }

    [XmlElement("Anagrafica")]
    public LegalName LegalName { get; set; } = new();
}

/// <summary>
/// Root header for Ordinary Invoices (<FatturaElettronicaHeader>).
/// </summary>
public class InvoiceHeader
{
    [XmlElement("DatiTrasmissione")]
    public TransmissionData TransmissionData { get; set; } = new();

    [XmlElement("CedentePrestatore")]
    public Supplier Supplier { get; set; } = new();

    [XmlElement("RappresentanteFiscale")]
    public TaxRepresentative? TaxRepresentative { get; set; }

    [XmlElement("CessionarioCommittente")]
    public Customer Customer { get; set; } = new();

    [XmlElement("TerzoIntermediarioOSoggettoEmittente")]
    public ThirdPartyIntermediary? ThirdPartyIntermediary { get; set; }

    [XmlElement("SoggettoEmittente")]
    public string? IssuingSubject { get; set; }
}
