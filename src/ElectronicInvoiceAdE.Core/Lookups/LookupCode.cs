namespace ElectronicInvoiceAdE.Lookups;

/// <summary>
/// Represents a standardized code entry with an alphanumeric code, an English name, and English and Italian descriptions.
/// </summary>
public record LookupCode(string Code, string Name, string EnglishDescription, string ItalianDescription)
{
    public override string ToString() => $"{Code} - {Name}";
}
