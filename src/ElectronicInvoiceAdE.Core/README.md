# ElectronicInvoiceAdE.Core

[![NuGet](https://img.shields.io/nuget/v/ElectronicInvoiceAdE.Core.svg)](https://www.nuget.org/packages/ElectronicInvoiceAdE.Core/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/Ub4thaan/ElectronicInvoiceAdE/blob/main/LICENSE)

`ElectronicInvoiceAdE.Core` provides the core strongly-typed C# domain models, tax catalog constants, and lookups for Italian Electronic Invoicing (**Fatturazione Elettronica**), fully compliant with **Agenzia delle Entrate Technical Specifications v1.9.1**.

This module is dependency-free (targeting .NET 8.0, .NET 9.0, and .NET 10.0 with C# 13) and defines the domain entities for both **Ordinary** (`FatturaElettronica` / `FPR12` / `FPA12`) and **Simplified** (`FatturaElettronicaSemplificata` / `FSM10`) invoices.

---

## Key Features

- **English Domain API**: Idiomatic, clean domain terminology (`OrdinaryInvoice`, `SimplifiedInvoice`, `Supplier`, `Customer`, `InvoiceBody`, `InvoiceLineItem`, `VatSummary`) with 1-to-1 fidelity to ministerial XML tags.
- **Comprehensive Fiscal Catalogs**:
  - `DocumentType`: TD01 (Invoice), TD04 (Credit Note), TD16–TD28 (Reverse Charge, Self-invoicing, Sanctions), TD07–TD09 (Simplified).
  - `TaxRegime`: RF01 (Ordinary), RF02 (Simplified), RF19 (Flat-rate / Regime Forfettario), and all ministerial regimes up to RF20.
  - `VatNature`: N1 (Excluded), N2 (Not subject), N3 (Non-taxable), N4 (Exempt), N5 (Margin regime), N6 (Reverse charge), N7 (Other EU VAT paid).
  - `PaymentMethod`: MP01 (Cash), MP02 (Check), MP05 (Bank Transfer / Bonifico), MP08 (Payment Card), up to MP23 (PagoPA).
  - `PensionFundType`: TC01–TC22 covering INPS, ENPAM, INARCASSA, CNPADC, and all professional funds.
  - `WithholdingType`: RT01 (Individuals), RT02 (Legal entities), RT03–RT06 (INPS, ENASARCO contributions).

---

## Installation

```shell
dotnet add package ElectronicInvoiceAdE.Core
```

---

## Quick Example

```csharp
using ElectronicInvoiceAdE.Core.Lookups;
using ElectronicInvoiceAdE.Core.Ordinary;
using ElectronicInvoiceAdE.Core.Ordinary.Bodies;
using ElectronicInvoiceAdE.Core.Ordinary.Headers;

var invoice = new OrdinaryInvoice
{
    Header = new Header
    {
        TransmissionData = new TransmissionData
        {
            Format = TransmissionFormat.Fpr12,
            CountryCode = "IT",
            TransmitterId = "01234567890",
            TransmissionNumber = "00001"
        },
        Supplier = new Supplier
        {
            Identification = new Identification
            {
                CountryCode = "IT",
                TaxId = "01234567890",
                LegalName = "Acme Supplies S.r.l."
            },
            TaxRegime = TaxRegime.Ordinary
        },
        Customer = new Customer
        {
            Identification = new CustomerIdentification
            {
                CountryCode = "IT",
                TaxId = "09876543210",
                LegalName = "Client Spa"
            }
        }
    }
};
```

---

## Part of ElectronicInvoiceAdE

This package is part of the modular [ElectronicInvoiceAdE](https://github.com/Ub4thaan/ElectronicInvoiceAdE) suite:
- `ElectronicInvoiceAdE`: Complete high-level facade.
- `ElectronicInvoiceAdE.Core`: Domain models and catalogs (this package).
- `ElectronicInvoiceAdE.Serialization`: High-performance XML/JSON serialization.
- `ElectronicInvoiceAdE.Validation`: Fiscal checksums (Partita IVA, Codice Fiscale) and official SdI rules.
- `ElectronicInvoiceAdE.Security`: Digital signature (.p7m / CAdES-BES) extraction and ZIP archive processing.
- `ElectronicInvoiceAdE.Data`: Entity Framework Core relational mappings and DbContext.
