using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Serialization;
using ElectronicInvoiceAdE.Simplified;

namespace ElectronicInvoiceAdE.Security;

/// <summary>
/// Digital signature info extracted from a CAdES-BES enveloped electronic invoice (.p7m).
/// </summary>
public class SignatureInfo
{
    public string Subject { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public DateTime NotBefore { get; set; }
    public DateTime NotAfter { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
    public string Thumbprint { get; set; } = string.Empty;
    public DateTime? SigningTime { get; set; }
}

/// <summary>
/// Result of unpacking a signed .p7m invoice container.
/// </summary>
public class SignedInvoicePackage
{
    public byte[] XmlContent { get; set; } = Array.Empty<byte>();
    public List<SignatureInfo> Signatures { get; set; } = new();

    public object ReadInvoice()
    {
        using var ms = new MemoryStream(XmlContent);
        return InvoiceReader.Read(ms);
    }

    public OrdinaryInvoice ReadOrdinary()
    {
        using var ms = new MemoryStream(XmlContent);
        return InvoiceReader.ReadOrdinary(ms);
    }

    public SimplifiedInvoice ReadSimplified()
    {
        using var ms = new MemoryStream(XmlContent);
        return InvoiceReader.ReadSimplified(ms);
    }
}

/// <summary>
/// Helper to inspect and unwrap CAdES-BES digitally signed (.xml.p7m / .p7m) invoices.
/// Supports both raw DER binary and Base64-encoded PKCS#7 structures.
/// </summary>
public static class SignedInvoiceReader
{
    /// <summary>
    /// Checks whether the given file or stream is likely a .p7m signed envelope.
    /// </summary>
    public static bool IsSignedInvoice(string filePath)
    {
        if (filePath.EndsWith(".p7m", StringComparison.OrdinalIgnoreCase))
            return true;

        using var stream = File.OpenRead(filePath);
        return IsSignedInvoice(stream);
    }

    public static bool IsSignedInvoice(Stream stream)
    {
        var startPos = stream.CanSeek ? stream.Position : 0;
        var header = new byte[16];
        var read = stream.Read(header, 0, header.Length);
        if (stream.CanSeek)
            stream.Seek(startPos, SeekOrigin.Begin);

        if (read < 2)
            return false;

        // ASN.1 SEQUENCE (0x30) or Base64 (starts with M or -)
        return header[0] == 0x30 || (header[0] == 'M' && header[1] == 'I') || (header[0] == '-');
    }

    public static SignedInvoicePackage Unpack(string filePath)
    {
        var bytes = File.ReadAllBytes(filePath);
        return Unpack(bytes);
    }

    public static SignedInvoicePackage Unpack(Stream stream)
    {
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return Unpack(ms.ToArray());
    }

    public static SignedInvoicePackage Unpack(byte[] data)
    {
        var signedCms = new SignedCms();

        try
        {
            signedCms.Decode(data);
        }
        catch
        {
            // If direct binary decode failed, test Base64 encoding
            var text = Encoding.ASCII.GetString(data).Trim();
            text = text.Replace("-----BEGIN PKCS7-----", "")
                       .Replace("-----END PKCS7-----", "")
                       .Replace("\r", "")
                       .Replace("\n", "")
                       .Trim();
            var rawBytes = Convert.FromBase64String(text);
            signedCms = new SignedCms();
            signedCms.Decode(rawBytes);
        }

        var result = new SignedInvoicePackage
        {
            XmlContent = signedCms.ContentInfo.Content
        };

        foreach (var cert in signedCms.Certificates)
        {
            result.Signatures.Add(new SignatureInfo
            {
                Subject = cert.Subject,
                Issuer = cert.Issuer,
                NotBefore = cert.NotBefore,
                NotAfter = cert.NotAfter,
                SerialNumber = cert.SerialNumber,
                Thumbprint = cert.Thumbprint
            });
        }

        return result;
    }
}
