using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ElectronicInvoiceAdE.Common;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Simplified;

namespace ElectronicInvoiceAdE.Serialization;

/// <summary>
/// Writer for generating valid Italian electronic invoice XML documents per Technical Specifications v1.9.1.
/// </summary>
public static class InvoiceWriter
{
    private static readonly XNamespace PNamespaceV12 = XmlNamespaces.FatturaOrdinariaV12;
    private static readonly XNamespace PNamespaceV10 = XmlNamespaces.FatturaSemplificataV10;
    private static readonly XNamespace DsNamespace = XmlNamespaces.XmlDsig;
    private static readonly XNamespace XsiNamespace = XmlNamespaces.XmlSchemaInstance;

    /// <summary>
    /// Converts an Ordinary Invoice to its XML string representation.
    /// </summary>
    public static string ToXml(OrdinaryInvoice invoice, bool indented = true)
    {
        var doc = BuildXDocument(invoice);
        var sb = new StringBuilder();
        var settings = new XmlWriterSettings
        {
            Indent = indented,
            Encoding = new UTF8Encoding(false),
            OmitXmlDeclaration = false
        };

        using var writer = XmlWriter.Create(sb, settings);
        doc.WriteTo(writer);
        writer.Flush();
        return sb.ToString();
    }

    /// <summary>
    /// Writes an Ordinary Invoice to a file on disk.
    /// </summary>
    public static void Write(OrdinaryInvoice invoice, string filePath, bool indented = true)
    {
        var dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        using var stream = File.Create(filePath);
        Write(invoice, stream, indented);
    }

    /// <summary>
    /// Writes an Ordinary Invoice to a stream.
    /// </summary>
    public static void Write(OrdinaryInvoice invoice, Stream stream, bool indented = true)
    {
        var doc = BuildXDocument(invoice);
        var settings = new XmlWriterSettings
        {
            Indent = indented,
            Encoding = new UTF8Encoding(false),
            OmitXmlDeclaration = false
        };

        using var writer = XmlWriter.Create(stream, settings);
        doc.WriteTo(writer);
        writer.Flush();
    }

    /// <summary>
    /// Converts a Simplified Invoice to its XML string representation.
    /// </summary>
    public static string ToXml(SimplifiedInvoice invoice, bool indented = true)
    {
        var doc = BuildXDocument(invoice);
        var sb = new StringBuilder();
        var settings = new XmlWriterSettings
        {
            Indent = indented,
            Encoding = new UTF8Encoding(false),
            OmitXmlDeclaration = false
        };

        using var writer = XmlWriter.Create(sb, settings);
        doc.WriteTo(writer);
        writer.Flush();
        return sb.ToString();
    }

    /// <summary>
    /// Writes a Simplified Invoice to a file on disk.
    /// </summary>
    public static void Write(SimplifiedInvoice invoice, string filePath, bool indented = true)
    {
        var dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        using var stream = File.Create(filePath);
        Write(invoice, stream, indented);
    }

    /// <summary>
    /// Writes a Simplified Invoice to a stream.
    /// </summary>
    public static void Write(SimplifiedInvoice invoice, Stream stream, bool indented = true)
    {
        var doc = BuildXDocument(invoice);
        var settings = new XmlWriterSettings
        {
            Indent = indented,
            Encoding = new UTF8Encoding(false),
            OmitXmlDeclaration = false
        };

        using var writer = XmlWriter.Create(stream, settings);
        doc.WriteTo(writer);
        writer.Flush();
    }

    private static XDocument BuildXDocument(OrdinaryInvoice invoice)
    {
        var root = new XElement(PNamespaceV12 + "FatturaElettronica",
            new XAttribute(XNamespace.Xmlns + "p", PNamespaceV12),
            new XAttribute(XNamespace.Xmlns + "ds", DsNamespace),
            new XAttribute(XNamespace.Xmlns + "xsi", XsiNamespace),
            new XAttribute("versione", invoice.Version)
        );

        if (!string.IsNullOrEmpty(invoice.IssuingSystem))
            root.SetAttributeValue("SistemaEmittente", invoice.IssuingSystem);

        XmlMapper.SerializeInto(root, invoice, XNamespace.None);

        return new XDocument(
            new XDeclaration("1.0", "UTF-8", null),
            root
        );
    }

    private static XDocument BuildXDocument(SimplifiedInvoice invoice)
    {
        var root = new XElement(PNamespaceV10 + "FatturaElettronicaSemplificata",
            new XAttribute(XNamespace.Xmlns + "p", PNamespaceV10),
            new XAttribute(XNamespace.Xmlns + "ds", DsNamespace),
            new XAttribute(XNamespace.Xmlns + "xsi", XsiNamespace),
            new XAttribute("versione", invoice.Version)
        );

        if (!string.IsNullOrEmpty(invoice.IssuingSystem))
            root.SetAttributeValue("SistemaEmittente", invoice.IssuingSystem);

        XmlMapper.SerializeInto(root, invoice, XNamespace.None);

        return new XDocument(
            new XDeclaration("1.0", "UTF-8", null),
            root
        );
    }
}
