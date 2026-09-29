using System;
using System.Collections.Generic;
using System.IO;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Security;
using ElectronicInvoiceAdE.Serialization;
using ElectronicInvoiceAdE.Simplified;
using ElectronicInvoiceAdE.Validation;

namespace ElectronicInvoiceAdE;

/// <summary>
/// Primary entry point for loading and interacting with Italian electronic invoices (Ordinary &amp; Simplified).
/// </summary>
public static class ElectronicInvoiceDocument
{
    /// <summary>
    /// Loads an electronic invoice from a file on disk, transparently handling XML, .p7m (signed), and encodings.
    /// </summary>
    public static object Load(string filePath)
    {
        if (SignedInvoiceReader.IsSignedInvoice(filePath))
        {
            var package = SignedInvoiceReader.Unpack(filePath);
            return package.ReadInvoice();
        }

        return InvoiceReader.Read(filePath);
    }

    /// <summary>
    /// Loads an Ordinary Electronic Invoice from a file on disk.
    /// </summary>
    public static OrdinaryInvoice LoadOrdinary(string filePath)
    {
        var result = Load(filePath);
        if (result is OrdinaryInvoice ordinary)
            return ordinary;

        throw new InvalidOperationException($"Document at '{filePath}' is not an Ordinary Invoice (found {result.GetType().Name}).");
    }

    /// <summary>
    /// Loads a Simplified Electronic Invoice from a file on disk.
    /// </summary>
    public static SimplifiedInvoice LoadSimplified(string filePath)
    {
        var result = Load(filePath);
        if (result is SimplifiedInvoice simplified)
            return simplified;

        throw new InvalidOperationException($"Document at '{filePath}' is not a Simplified Invoice (found {result.GetType().Name}).");
    }

    /// <summary>
    /// Loads an electronic invoice from an XML string.
    /// </summary>
    public static object LoadFromXml(string xmlContent) => InvoiceReader.ReadXml(xmlContent);

    /// <summary>
    /// Loads an electronic invoice from a stream, transparently detecting .p7m signature or plain XML.
    /// </summary>
    public static object Load(Stream stream)
    {
        if (SignedInvoiceReader.IsSignedInvoice(stream))
        {
            var package = SignedInvoiceReader.Unpack(stream);
            return package.ReadInvoice();
        }

        return InvoiceReader.Read(stream);
    }

    /// <summary>
    /// Loads all invoices contained within a ZIP archive.
    /// </summary>
    public static List<ZipInvoiceEntry> LoadFromZip(string zipFilePath) => ZipInvoiceReader.ReadZip(zipFilePath);

    /// <summary>
    /// Loads all invoices contained within a ZIP stream.
    /// </summary>
    public static List<ZipInvoiceEntry> LoadFromZip(Stream zipStream) => ZipInvoiceReader.ReadZip(zipStream);
}
