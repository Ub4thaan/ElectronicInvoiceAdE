using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ElectronicInvoiceAdE.Serialization;

namespace ElectronicInvoiceAdE.Security;

public record ZipInvoiceEntry(string EntryName, object Invoice, bool WasSigned);

/// <summary>
/// Helper to extract and read electronic invoices packaged in ZIP archives.
/// </summary>
public static class ZipInvoiceReader
{
    public static List<ZipInvoiceEntry> ReadZip(string zipFilePath)
    {
        using var stream = File.OpenRead(zipFilePath);
        return ReadZip(stream);
    }

    public static List<ZipInvoiceEntry> ReadZip(Stream zipStream)
    {
        var results = new List<ZipInvoiceEntry>();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read, leaveOpen: true);

        foreach (var entry in archive.Entries)
        {
            var ext = Path.GetExtension(entry.FullName).ToLowerInvariant();
            if (ext is not (".xml" or ".p7m"))
                continue;

            using var entryStream = entry.Open();
            using var ms = new MemoryStream();
            entryStream.CopyTo(ms);
            var bytes = ms.ToArray();

            if (entry.FullName.EndsWith(".p7m", StringComparison.OrdinalIgnoreCase))
            {
                var package = SignedInvoiceReader.Unpack(bytes);
                var invoice = package.ReadInvoice();
                results.Add(new ZipInvoiceEntry(entry.FullName, invoice, true));
            }
            else
            {
                using var xmlMs = new MemoryStream(bytes);
                var invoice = InvoiceReader.Read(xmlMs);
                results.Add(new ZipInvoiceEntry(entry.FullName, invoice, false));
            }
        }

        return results;
    }
}
