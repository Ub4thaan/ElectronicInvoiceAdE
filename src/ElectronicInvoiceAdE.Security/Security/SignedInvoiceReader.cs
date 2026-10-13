using System;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace ElectronicInvoiceAdE.Security;

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
		ArgumentNullException.ThrowIfNull(filePath);
		if (filePath.EndsWith(".p7m", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}

		using var stream = File.OpenRead(filePath);
		return IsSignedInvoice(stream);
	}

	public static bool IsSignedInvoice(Stream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);
		var startPos = stream.CanSeek ? stream.Position : 0;
		var header = new byte[16];
		var read = stream.Read(header, 0, header.Length);
		if (stream.CanSeek)
		{
			stream.Seek(startPos, SeekOrigin.Begin);
		}

		if (read < 2)
		{
			return false;
		}

		// ASN.1 SEQUENCE (0x30) or Base64 (starts with M or -)
		return header[0] == 0x30 || header[0] == 'M' && header[1] == 'I' || header[0] == '-';
	}

	public static SignedInvoicePackage Unpack(string filePath)
	{
		ArgumentNullException.ThrowIfNull(filePath);
		var bytes = File.ReadAllBytes(filePath);
		return Unpack(bytes);
	}

	public static SignedInvoicePackage Unpack(Stream stream)
	{
		ArgumentNullException.ThrowIfNull(stream);
		using var ms = new MemoryStream();
		stream.CopyTo(ms);
		return Unpack(ms.ToArray());
	}

	public static SignedInvoicePackage Unpack(byte[] data)
	{
		ArgumentNullException.ThrowIfNull(data);
		var signedCms = new SignedCms();

		try
		{
			signedCms.Decode(data);
		}
		catch
		{
			// If direct binary decode failed, test Base64 encoding
			var text = Encoding.ASCII.GetString(data).Trim();
			text = text.Replace("-----BEGIN PKCS7-----", "", StringComparison.Ordinal)
					   .Replace("-----END PKCS7-----", "", StringComparison.Ordinal)
					   .Replace("\r", "", StringComparison.Ordinal)
					   .Replace("\n", "", StringComparison.Ordinal)
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
