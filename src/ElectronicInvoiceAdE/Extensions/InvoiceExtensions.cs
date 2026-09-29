using System.IO;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Serialization;
using ElectronicInvoiceAdE.Simplified;
using ElectronicInvoiceAdE.Validation;

namespace ElectronicInvoiceAdE.Extensions;

/// <summary>
/// Convenient extension methods for Ordinary and Simplified electronic invoices.
/// </summary>
public static class InvoiceExtensions
{
    /// <summary>Serializes an Ordinary Invoice to its official XML string representation.</summary>
    public static string ToXml(this OrdinaryInvoice invoice, bool indented = true) => InvoiceWriter.ToXml(invoice, indented);

    /// <summary>Serializes a Simplified Invoice to its official XML string representation.</summary>
    public static string ToXml(this SimplifiedInvoice invoice, bool indented = true) => InvoiceWriter.ToXml(invoice, indented);

    /// <summary>Saves an Ordinary Invoice as an XML file on disk.</summary>
    public static void Save(this OrdinaryInvoice invoice, string filePath, bool indented = true) => InvoiceWriter.Write(invoice, filePath, indented);

    /// <summary>Saves a Simplified Invoice as an XML file on disk.</summary>
    public static void Save(this SimplifiedInvoice invoice, string filePath, bool indented = true) => InvoiceWriter.Write(invoice, filePath, indented);

    /// <summary>Saves an Ordinary Invoice as XML to a stream.</summary>
    public static void Save(this OrdinaryInvoice invoice, Stream stream, bool indented = true) => InvoiceWriter.Write(invoice, stream, indented);

    /// <summary>Saves a Simplified Invoice as XML to a stream.</summary>
    public static void Save(this SimplifiedInvoice invoice, Stream stream, bool indented = true) => InvoiceWriter.Write(invoice, stream, indented);

    /// <summary>Serializes an Ordinary Invoice to a JSON string.</summary>
    public static string ToJson(this OrdinaryInvoice invoice, bool indented = true) => JsonSerialization.ToJson(invoice, indented);

    /// <summary>Serializes a Simplified Invoice to a JSON string.</summary>
    public static string ToJson(this SimplifiedInvoice invoice, bool indented = true) => JsonSerialization.ToJson(invoice, indented);

    /// <summary>Validates an Ordinary Invoice against official SDI v1.9.1 rules and Italian tax algorithms.</summary>
    public static ValidationResult Validate(this OrdinaryInvoice invoice) => InvoiceValidator.Validate(invoice);

    /// <summary>Validates a Simplified Invoice against official SDI v1.9.1 rules and Italian tax algorithms.</summary>
    public static ValidationResult Validate(this SimplifiedInvoice invoice) => InvoiceValidator.Validate(invoice);
}
