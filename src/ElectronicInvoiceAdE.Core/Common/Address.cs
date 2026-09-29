using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Common;

/// <summary>
/// Physical address (<Sede>, <IndirizzoResa>, <StabileOrganizzazione>).
/// </summary>
public class Address
{
    [XmlElement("Indirizzo")]
    public string Street { get; set; } = string.Empty;

    [XmlElement("NumeroCivico")]
    public string? StreetNumber { get; set; }

    [XmlElement("CAP")]
    public string PostalCode { get; set; } = string.Empty;

    [XmlElement("Comune")]
    public string Municipality { get; set; } = string.Empty;

    [XmlElement("Provincia")]
    public string? Province { get; set; }

    [XmlElement("Nazione")]
    public string CountryCode { get; set; } = "IT";

    public override string ToString()
    {
        var civic = string.IsNullOrWhiteSpace(StreetNumber) ? string.Empty : $" {StreetNumber}";
        var prov = string.IsNullOrWhiteSpace(Province) ? string.Empty : $" ({Province})";
        return $"{Street}{civic}, {PostalCode} {Municipality}{prov}, {CountryCode}";
    }
}
