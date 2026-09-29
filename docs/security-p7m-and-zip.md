# Security: P7M Envelope Unpacking & ZIP Processing

This guide covers the cryptographic and packaging utilities in `ElectronicInvoiceAdE.Security`, specifically focusing on **CAdES-BES PKCS#7 (`.p7m`)** envelope extraction and batch **ZIP archive** processing.

---

## 1. CAdES-BES Invoicing in Italy

In Italian electronic invoicing:
1. **Public Administration Invoices (`FPA12`)**: Legally require a digital signature by the transmitter or supplier using a qualified certificate (e.g. CNS, token, Smart Card, remote HSM).
2. **Private Invoices (`FPR12`)**: Often signed optionally for non-repudiation and integrity.
3. **Format**: The signature standard is **CAdES-BES** (Cryptographic Message Syntax, RFC 5652) containing an enveloped XML payload, producing files named `<FileName>.xml.p7m` or `<FileName>.p7m`.

---

## 2. Unpacking Signed Invoices (`SignedInvoiceReader`)

`SignedInvoiceReader` provides non-destructive, memory-efficient extraction of the enveloped XML payload and cryptographic metadata.

### ASN.1 DER vs. Base64 PEM Support

Signed files may be formatted in two ways:
- **Raw Binary ASN.1 DER**: The standard binary format (beginning with ASN.1 sequence byte `0x30`).
- **Base64 ASCII PEM**: Wrapped in `-----BEGIN PKCS7-----` headers.

`SignedInvoiceReader` automatically tests the binary sequence and transparently falls back to Base64 PEM decoding if binary parsing fails:

```csharp
using ElectronicInvoiceAdE.Security;

// Check if a file or stream contains a digital signature
bool isSigned = SignedInvoiceReader.IsSignedInvoice("IT01234567890_00001.xml.p7m");

// Unpack the container
SignedInvoicePackage package = SignedInvoiceReader.Unpack("IT01234567890_00001.xml.p7m");

// Access extracted raw XML bytes
byte[] xmlBytes = package.XmlContent;

// Parse directly to domain objects
OrdinaryInvoice ordinary = package.ReadOrdinary();
```

---

## 3. Digital Signature & Certificate Metadata Inspection

Every `SignedInvoicePackage` exposes the collection of digital certificates and signatures found in the CMS container:

```csharp
foreach (SignatureInfo signature in package.Signatures)
{
    Console.WriteLine($"Signer Subject: {signature.Subject}");
    Console.WriteLine($"Certificate Issuer: {signature.Issuer}");
    Console.WriteLine($"Serial Number: {signature.SerialNumber}");
    Console.WriteLine($"SHA-1 Thumbprint: {signature.Thumbprint}");
    Console.WriteLine($"Valid From: {signature.NotBefore:yyyy-MM-dd}");
    Console.WriteLine($"Valid Until: {signature.NotAfter:yyyy-MM-dd}");
    
    if (signature.SigningTime.HasValue)
    {
        Console.WriteLine($"Signing Timestamp: {signature.SigningTime.Value:yyyy-MM-dd HH:mm:ss}");
    }
}
```

This enables automated auditing, checking certificate expiration dates, and verifying that the signer identity matches the designated invoice transmitter.

---

## 4. Multi-Invoice ZIP Archive Reading (`ZipInvoiceReader`)

In real-world accounting workflows, accountants and companies frequently receive bulk ZIP files containing hundreds or thousands of mixed `.xml` and `.xml.p7m` files (e.g. from the Agenzia delle Entrate "Cassetto Fiscale" portal).

`ZipInvoiceReader` processes archives using forward-only decompression without writing intermediate temporary files to disk:

```csharp
using ElectronicInvoiceAdE.Security;
using ElectronicInvoiceAdE.Ordinary;

// Open from file path or stream
List<ZipInvoiceEntry> entries = ZipInvoiceReader.ReadZip("batch_2026_q1.zip");

foreach (ZipInvoiceEntry entry in entries)
{
    Console.WriteLine($"File: {entry.EntryName}");
    Console.WriteLine($"Was Digitally Signed: {entry.WasSigned}");

    if (entry.Invoice is OrdinaryInvoice invoice)
    {
        var doc = invoice.PrimaryBody.GeneralData.DocumentGeneralData;
        Console.WriteLine($"  Invoice {doc.Number} of {doc.Date:yyyy-MM-dd} (Total: {doc.TotalAmount:C})");
    }
}
```

### Memory Considerations
- Operates entirely in memory using `System.IO.Compression.ZipArchive`.
- Leaves streams open for reuse (`leaveOpen: true`).
- Filters out non-invoice files automatically (ignores files that do not end in `.xml` or `.p7m`).
