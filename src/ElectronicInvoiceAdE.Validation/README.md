# ElectronicInvoiceAdE.Validation

[![NuGet](https://img.shields.io/nuget/v/ElectronicInvoiceAdE.Validation.svg)](https://www.nuget.org/packages/ElectronicInvoiceAdE.Validation/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/Ub4thaan/ElectronicInvoiceAdE/blob/main/LICENSE)

`ElectronicInvoiceAdE.Validation` provides pure C# offline fiscal validation rules and checksum algorithms for Italian Electronic Invoicing (**Fatturazione Elettronica**), conforming to official **Agenzia delle Entrate Technical Specifications v1.9.1** and **Sistema di Interscambio (SdI)** standards.

---

## Key Features

- **SdI Business Rules Engine**: Validates invoices against official ministerial rejection error codes:
  - Error `00327`: Missing or invalid VAT group identification.
  - Error `00400` / `00401`: Inconsistency between 0% VAT rate and ministerial VAT Nature (`N1`–`N7`).
  - Cross-field validation of totals, tax summaries, rounding, and withholding amounts.
- **Italian Tax ID Algorithmic Checksums**:
  - **Partita IVA**: 11-digit modified Luhn checksum verification with office code checks.
  - **Codice Fiscale**: Alphanumeric 16-character fiscal code validator with complete ministerial odd/even character weight tables and full support for omocodia alphanumeric substitutions.
- **Structured Validation Results**: Clean separation of blocking errors (`Errors`) and non-blocking advisories (`Warnings`).

---

## Installation

```shell
dotnet add package ElectronicInvoiceAdE.Validation
```

---

## Quick Example

```csharp
using ElectronicInvoiceAdE.Core.Ordinary;
using ElectronicInvoiceAdE.Validation;

OrdinaryInvoice invoice = ...;

var validator = new InvoiceValidator();
ValidationResult result = validator.Validate(invoice);

if (!result.IsValid)
{
    Console.WriteLine("Invoice has validation errors:");
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"  [SdI {error.Code}] {error.Message} (Field: {error.PropertyName})");
    }
}

// Direct checksum verification
bool isValidPiva = ItalianTaxValidation.IsValidPartitaIva("01234567890");
bool isValidCf = ItalianTaxValidation.IsValidCodiceFiscale("RSSMRA85M01H501Z");
```

---

## Part of ElectronicInvoiceAdE

This package is part of the modular [ElectronicInvoiceAdE](https://github.com/Ub4thaan/ElectronicInvoiceAdE) suite:
- `ElectronicInvoiceAdE`: Complete high-level facade.
- `ElectronicInvoiceAdE.Core`: Domain models and catalogs.
- `ElectronicInvoiceAdE.Serialization`: High-performance XML/JSON serialization.
- `ElectronicInvoiceAdE.Validation`: Fiscal checksums and SdI rules (this package).
- `ElectronicInvoiceAdE.Security`: Digital signature (.p7m / CAdES-BES) extraction and ZIP archive processing.
- `ElectronicInvoiceAdE.Data`: Entity Framework Core relational mappings and DbContext.
