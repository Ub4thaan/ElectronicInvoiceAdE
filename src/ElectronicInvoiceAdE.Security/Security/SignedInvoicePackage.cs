using System;
using System.Collections.Generic;
using System.IO;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Serialization;
using ElectronicInvoiceAdE.Simplified;

namespace ElectronicInvoiceAdE.Security;

/// <summary>
/// Result of unpacking a signed .p7m invoice container.
/// </summary>
public class SignedInvoicePackage
{
	public byte[] XmlContent { get; set; } = Array.Empty<byte>();
	public List<SignatureInfo> Signatures { get; set; } = [];

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
