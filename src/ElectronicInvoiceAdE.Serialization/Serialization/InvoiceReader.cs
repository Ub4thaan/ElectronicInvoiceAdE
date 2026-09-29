using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using ElectronicInvoiceAdE.Common;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Simplified;

namespace ElectronicInvoiceAdE.Serialization;

/// <summary>
/// Reader for Italian electronic invoice documents (Ordinary and Simplified).
/// Automatically handles encodings, namespaces, and format detection.
/// </summary>
public static class InvoiceReader
{
    static InvoiceReader()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    /// <summary>
    /// Reads an electronic invoice file from disk, returning either an OrdinaryInvoice or SimplifiedInvoice.
    /// </summary>
    public static object Read(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return Read(stream);
    }

    /// <summary>
    /// Reads an electronic invoice from a stream, returning either an OrdinaryInvoice or SimplifiedInvoice.
    /// </summary>
    public static object Read(Stream stream)
    {
        var doc = XDocument.Load(stream);
        return Read(doc);
    }

    /// <summary>
    /// Reads an electronic invoice from an XML string, returning either an OrdinaryInvoice or SimplifiedInvoice.
    /// </summary>
    public static object ReadXml(string xmlContent)
    {
        var doc = XDocument.Parse(xmlContent);
        return Read(doc);
    }

    /// <summary>
    /// Reads an electronic invoice from an XDocument.
    /// </summary>
    public static object Read(XDocument document)
    {
        if (document.Root == null)
            throw new InvalidOperationException("XML document has no root element.");

        if (IsSimplified(document.Root))
            return ReadSimplified(document);

        return ReadOrdinary(document);
    }

    /// <summary>
    /// Reads an Ordinary Electronic Invoice (FPA12 / FPR12).
    /// </summary>
    public static OrdinaryInvoice ReadOrdinary(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return ReadOrdinary(stream);
    }

    /// <summary>
    /// Reads an Ordinary Electronic Invoice from a stream.
    /// </summary>
    public static OrdinaryInvoice ReadOrdinary(Stream stream)
    {
        var doc = XDocument.Load(stream);
        return ReadOrdinary(doc);
    }

    /// <summary>
    /// Reads an Ordinary Electronic Invoice from an XML string.
    /// </summary>
    public static OrdinaryInvoice ReadOrdinaryXml(string xmlContent)
    {
        var doc = XDocument.Parse(xmlContent);
        return ReadOrdinary(doc);
    }

    /// <summary>
    /// Reads an Ordinary Electronic Invoice from an XDocument.
    /// </summary>
    public static OrdinaryInvoice ReadOrdinary(XDocument document)
    {
        if (document.Root == null)
            throw new InvalidOperationException("XML document has no root element.");

        return XmlMapper.Deserialize<OrdinaryInvoice>(document.Root);
    }

    /// <summary>
    /// Reads a Simplified Electronic Invoice (FSM10).
    /// </summary>
    public static SimplifiedInvoice ReadSimplified(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return ReadSimplified(stream);
    }

    /// <summary>
    /// Reads a Simplified Electronic Invoice from a stream.
    /// </summary>
    public static SimplifiedInvoice ReadSimplified(Stream stream)
    {
        var doc = XDocument.Load(stream);
        return ReadSimplified(doc);
    }

    /// <summary>
    /// Reads a Simplified Electronic Invoice from an XML string.
    /// </summary>
    public static SimplifiedInvoice ReadSimplifiedXml(string xmlContent)
    {
        var doc = XDocument.Parse(xmlContent);
        return ReadSimplified(doc);
    }

    /// <summary>
    /// Reads a Simplified Electronic Invoice from an XDocument.
    /// </summary>
    public static SimplifiedInvoice ReadSimplified(XDocument document)
    {
        if (document.Root == null)
            throw new InvalidOperationException("XML document has no root element.");

        return XmlMapper.Deserialize<SimplifiedInvoice>(document.Root);
    }

    private static bool IsSimplified(XElement root)
    {
        if (string.Equals(root.Name.LocalName, "FatturaElettronicaSemplificata", StringComparison.OrdinalIgnoreCase))
            return true;

        var version = root.Attributes().FirstOrDefault(a => string.Equals(a.Name.LocalName, "versione", StringComparison.OrdinalIgnoreCase))?.Value;
        return string.Equals(version, "FSM10", StringComparison.OrdinalIgnoreCase);
    }
}
