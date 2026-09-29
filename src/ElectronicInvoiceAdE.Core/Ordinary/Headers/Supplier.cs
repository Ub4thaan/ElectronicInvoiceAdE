using System;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;
using ElectronicInvoiceAdE.Lookups;

namespace ElectronicInvoiceAdE.Ordinary.Headers;

/// <summary>
/// Identification data of the supplier/seller (<DatiAnagrafici> in <CedentePrestatore>).
/// </summary>
public class SupplierIdentification
{
    [XmlElement("IdFiscaleIVA")]
    public TaxId TaxId { get; set; } = new();

    [XmlElement("CodiceFiscale")]
    public string? FiscalCode { get; set; }

    [XmlElement("Anagrafica")]
    public LegalName LegalName { get; set; } = new();

    [XmlElement("AlboProfessionale")]
    public string? ProfessionalRoll { get; set; }

    [XmlElement("ProvinciaAlbo")]
    public string? ProfessionalRollProvince { get; set; }

    [XmlElement("NumeroIscrizioneAlbo")]
    public string? ProfessionalRollNumber { get; set; }

    [XmlElement("DataIscrizioneAlbo")]
    public DateTime? ProfessionalRollDate { get; set; }

    [XmlElement("RegimeFiscale")]
    public string TaxRegime { get; set; } = Lookups.TaxRegime.Ordinary;
}

/// <summary>
/// Seller / Supplier data (<CedentePrestatore>).
/// </summary>
public class Supplier
{
    [XmlElement("DatiAnagrafici")]
    public SupplierIdentification Identification { get; set; } = new();

    [XmlElement("Sede")]
    public Address Address { get; set; } = new();

    [XmlElement("StabileOrganizzazione")]
    public Address? PermanentEstablishment { get; set; }

    [XmlElement("IscrizioneREA")]
    public ReaRegistration? ReaRegistration { get; set; }

    [XmlElement("Contatti")]
    public ContactInfo? Contacts { get; set; }

    [XmlElement("RiferimentoAmministrazione")]
    public string? AdministrativeReference { get; set; }
}
