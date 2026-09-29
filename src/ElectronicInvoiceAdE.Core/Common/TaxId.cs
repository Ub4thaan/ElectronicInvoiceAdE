using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Common;

/// <summary>
/// Tax identification number (<IdFiscaleIVA>).
/// Includes 2-letter ISO country code and VAT / tax code.
/// </summary>
public class TaxId
{
    public TaxId() { }

    public TaxId(string countryCode, string code)
    {
        CountryCode = countryCode;
        Code = code;
    }

    [XmlElement("IdPaese")]
    public string CountryCode { get; set; } = "IT";

    [XmlElement("IdCodice")]
    public string Code { get; set; } = string.Empty;

    public override string ToString() => $"{CountryCode}{Code}";
}
