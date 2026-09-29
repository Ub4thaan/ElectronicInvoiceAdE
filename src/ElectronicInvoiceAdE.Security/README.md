# ElectronicInvoiceAdE.Security

[![NuGet](https://img.shields.io/nuget/v/ElectronicInvoiceAdE.Security.svg)](https://www.nuget.org/packages/ElectronicInvoiceAdE.Security/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/Ub4thaan/ElectronicInvoiceAdE/blob/main/LICENSE)

`ElectronicInvoiceAdE.Security` provides cryptographic tools and archive unpackers for Italian Electronic Invoicing (**Fatturazione Elettronica**), specializing in CAdES-BES PKCS#7 (`.p7m`) digital signatures and multi-invoice ZIP containers.

---

## Key Features

- **CAdES-BES PKCS#7 Unpacking (`.xml.p7m` / `.p7m`)**:
  - Automatically handles both raw binary DER and Base64 PEM encoded digital signature envelopes.
  - Extracts the underlying XML payload without invoking external native tools or shell commands.
- **X.509 Certificate Inspection**:
  - Exposes signer certificate details: subject, issuer, serial number, thumbprint, and validity windows.
- **Batch ZIP Extraction**:
  - Extracts and parses mixed ZIP archives containing both plain XML and `.p7m` signed files.

---

## Installation

```shell
dotnet add package ElectronicInvoiceAdE.Security
```

---

## Quick Example

```csharp
using ElectronicInvoiceAdE.Security;

// Read and unpack a signed .p7m invoice
SignedInvoiceReader reader = new SignedInvoiceReader();
SignedInvoiceResult result = reader.ReadFromFile("IT01234567890_00001.xml.p7m");

Console.WriteLine($"Signer Subject: {result.SignerCertificate?.Subject}");
Console.WriteLine($"Valid Until:    {result.SignerCertificate?.NotAfter}");

// Unpacked XML content
string rawXml = result.XmlContent;

// Process a batch ZIP archive
ZipInvoiceReader zipReader = new ZipInvoiceReader();
List<ZipInvoiceEntry> entries = zipReader.ReadZip("batch.zip");

foreach (var entry in entries)
{
    Console.WriteLine($"Extracted: {entry.EntryName} (Signed: {entry.WasSigned})");
}
```

---

## Part of ElectronicInvoiceAdE

This package is part of the modular [ElectronicInvoiceAdE](https://github.com/Ub4thaan/ElectronicInvoiceAdE) suite:
- `ElectronicInvoiceAdE`: Complete high-level facade.
- `ElectronicInvoiceAdE.Core`: Domain models and catalogs.
- `ElectronicInvoiceAdE.Serialization`: High-performance XML/JSON serialization.
- `ElectronicInvoiceAdE.Validation`: Fiscal checksums and SdI rules.
- `ElectronicInvoiceAdE.Security`: Digital signature (.p7m / CAdES-BES) extraction and ZIP archive processing (this package).
- `ElectronicInvoiceAdE.Data`: Entity Framework Core relational mappings and DbContext.
