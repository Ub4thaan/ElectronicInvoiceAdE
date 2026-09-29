using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Common;

/// <summary>
/// Identification data for an entity or individual (<Anagrafica>).
/// </summary>
public class LegalName
{
    [XmlElement("Denominazione")]
    public string? CompanyName { get; set; }

    [XmlElement("Nome")]
    public string? FirstName { get; set; }

    [XmlElement("Cognome")]
    public string? LastName { get; set; }

    [XmlElement("Titolo")]
    public string? Title { get; set; }

    [XmlElement("CodEORI")]
    public string? EoriCode { get; set; }

    [XmlIgnore]
    public string FullName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(CompanyName))
                return CompanyName!;
            var parts = new[] { FirstName, LastName }.Where(p => !string.IsNullOrWhiteSpace(p));
            return string.Join(" ", parts) ?? string.Empty;
        }
    }

    public override string ToString() => FullName;
}
