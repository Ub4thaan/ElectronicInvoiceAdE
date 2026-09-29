# ElectronicInvoiceAdE.Data

[![NuGet](https://img.shields.io/nuget/v/ElectronicInvoiceAdE.Data.svg)](https://www.nuget.org/packages/ElectronicInvoiceAdE.Data/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/Ub4thaan/ElectronicInvoiceAdE/blob/main/LICENSE)

`ElectronicInvoiceAdE.Data` provides Entity Framework Core relational mappings, database entity models, and a configurable `DbContext` for querying and persisting Italian Electronic Invoices (**Fatturazione Elettronica**).

---

## Key Features

- **Relational Schema Mappings**: Normalized, queryable EF Core entities corresponding to:
  - Header data: Supplier, Customer, Transmitter, Third-party intermediaries.
  - Body data: Document general metadata, line items, VAT summaries, payment details.
- **Pre-Configured `DbContext`**: `ElectronicInvoiceDbContext` with sensible index definitions for fast lookups by invoice number, date, and tax identifiers (Partita IVA / Codice Fiscale).
- **Multi-Targeting**: Supports EF Core 8.0, 9.0, and 10.0 aligned with your target runtime.
- **Extensible Schema**: Designed to inherit and customize table names, schemas, or foreign keys in your own application `DbContext`.

---

## Installation

```shell
dotnet add package ElectronicInvoiceAdE.Data
```

---

## Quick Example

```csharp
using Microsoft.EntityFrameworkCore;
using ElectronicInvoiceAdE.Data;

var options = new DbContextOptionsBuilder<ElectronicInvoiceDbContext>()
    .UseSqlServer("Server=...;Database=Invoices;Trusted_Connection=True;")
    .Options;

using var db = new ElectronicInvoiceDbContext(options);

// Query invoices by supplier P.IVA
var invoices = await db.Invoices
    .Include(i => i.Lines)
    .Include(i => i.VatSummaries)
    .Where(i => i.SupplierVatNumber == "01234567890")
    .ToListAsync();
```

---

## Part of ElectronicInvoiceAdE

This package is part of the modular [ElectronicInvoiceAdE](https://github.com/Ub4thaan/ElectronicInvoiceAdE) suite:
- `ElectronicInvoiceAdE`: Complete high-level facade.
- `ElectronicInvoiceAdE.Core`: Domain models and catalogs.
- `ElectronicInvoiceAdE.Serialization`: High-performance XML/JSON serialization.
- `ElectronicInvoiceAdE.Validation`: Fiscal checksums and SdI rules.
- `ElectronicInvoiceAdE.Security`: Digital signature (.p7m / CAdES-BES) extraction and ZIP archive processing.
- `ElectronicInvoiceAdE.Data`: Entity Framework Core relational mappings and DbContext (this package).
