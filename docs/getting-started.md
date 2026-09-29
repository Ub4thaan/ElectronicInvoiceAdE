# Getting Started with ElectronicInvoiceAdE

This guide walks you through installing **ElectronicInvoiceAdE**, loading electronic invoices in various formats, running offline fiscal validations, and exporting invoices to XML or JSON.

---

## 1. Prerequisites

- **.NET SDK**: Compatible with **.NET 8.0, .NET 9.0, or .NET 10.0**.
- **C# Language Version**: C# 13.0 or higher.
- **Operating System**: Cross-platform (Windows, Linux, macOS).

---

## 2. Installation

Install the unified metapackage via the .NET CLI:

```shell
dotnet add package ElectronicInvoiceAdE
```

Or install specific modular packages if you are building decoupled microservices:

```shell
# For core domain models and fiscal lookup constants
dotnet add package ElectronicInvoiceAdE.Core

# For XML and JSON parsing and writing
dotnet add package ElectronicInvoiceAdE.Serialization

# For offline validation and check-digit algorithms
dotnet add package ElectronicInvoiceAdE.Validation

# For P7M CAdES-BES unpacking and ZIP reading
dotnet add package ElectronicInvoiceAdE.Security

# For Entity Framework Core relational database mapping
dotnet add package ElectronicInvoiceAdE.Data
```

---

## 3. Basic Usage Walkthrough

### Step 1: Loading an Invoice

The primary entry point is the static class `ElectronicInvoiceDocument`. It transparently inspects the file or stream and handles:
- Plain XML files (`.xml`).
- Digitally signed CAdES-BES envelopes (`.xml.p7m` or `.p7m`).
- Legacy character encodings (such as `windows-1252` and `ISO-8859-1`).

```csharp
using ElectronicInvoiceAdE;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Simplified;

// Load an invoice from disk (transparent format detection)
object document = ElectronicInvoiceDocument.Load("IT01234567890_00001.xml.p7m");

if (document is OrdinaryInvoice ordinary)
{
    // Process ordinary invoice (B2B, B2C, or PA)
    ProcessOrdinaryInvoice(ordinary);
}
else if (document is SimplifiedInvoice simplified)
{
    // Process simplified invoice
    ProcessSimplifiedInvoice(simplified);
}
```

If you know the expected invoice type in advance, you can use strongly typed convenience methods:

```csharp
// Throws InvalidOperationException if the file is not an Ordinary Invoice
OrdinaryInvoice ordinary = ElectronicInvoiceDocument.LoadOrdinary("invoice.xml");

// Throws InvalidOperationException if the file is not a Simplified Invoice
SimplifiedInvoice simplified = ElectronicInvoiceDocument.LoadSimplified("simplified.xml");
```

---

### Step 2: Accessing Domain Properties

Domain models use clean, idiomatic English property names mapped directly to the Italian SdI technical tags:

```csharp
void ProcessOrdinaryInvoice(OrdinaryInvoice invoice)
{
    // Transmission header data
    var transmission = invoice.Header.TransmissionData;
    Console.WriteLine($"Transmission Format: {transmission.Format}"); // FPR12 or FPA12
    Console.WriteLine($"Recipient Code:      {transmission.RecipientCode}");
    Console.WriteLine($"Transmission Seq:    {transmission.TransmissionSequence}");

    // Supplier (Cedente / Prestatore)
    var supplier = invoice.Header.Supplier;
    Console.WriteLine($"Supplier Name:  {supplier.Identification.LegalName.CompanyName}");
    Console.WriteLine($"Supplier P.IVA: {supplier.Identification.TaxId.CountryCode}{supplier.Identification.TaxId.Code}");
    Console.WriteLine($"Tax Regime:     {supplier.Identification.TaxRegime}"); // e.g. RF01
    Console.WriteLine($"Address:        {supplier.Address.Street}, {supplier.Address.PostalCode} {supplier.Address.Municipality}");

    // Customer (Cessionario / Committente)
    var customer = invoice.Header.Customer;
    Console.WriteLine($"Customer Name:  {customer.Identification.LegalName?.CompanyName}");
    Console.WriteLine($"Customer P.IVA: {customer.Identification.TaxId?.Code}");
    Console.WriteLine($"Customer CF:    {customer.Identification.FiscalCode}");

    // Invoice Body (most invoices contain 1 primary body)
    var body = invoice.PrimaryBody;
    var docData = body.GeneralData.DocumentGeneralData;
    Console.WriteLine($"Document Type:   {docData.DocumentType}"); // e.g. TD01
    Console.WriteLine($"Invoice Number:  {docData.Number}");
    Console.WriteLine($"Date:            {docData.Date:yyyy-MM-dd}");
    Console.WriteLine($"Total Amount:    {docData.TotalAmount:C}");

    // Invoice Lines
    Console.WriteLine("Line items:");
    foreach (var line in body.GoodsAndServices.Lines)
    {
        Console.WriteLine($"  [{line.LineNumber}] {line.Description} — Qty: {line.Quantity} x {line.UnitPrice:C} = {line.TotalPrice:C} (VAT: {line.VatRate}%)");
    }

    // VAT Summaries (DatiRiepilogo)
    Console.WriteLine("VAT Summaries:");
    foreach (var vat in body.GoodsAndServices.VatSummaries)
    {
        Console.WriteLine($"  VAT Rate: {vat.VatRate}% | Taxable: {vat.TaxableAmount:C} | Tax: {vat.TaxAmount:C} | Nature: {vat.VatNature}");
    }
}
```

---

### Step 3: Validating Invoices Offline

Before sending an invoice to SdI or before persisting it in your database, execute offline validation against SdI v1.9.1 rules:

```csharp
using ElectronicInvoiceAdE.Extensions;
using ElectronicInvoiceAdE.Validation;

ValidationResult validation = ordinary.Validate();

if (validation.IsValid)
{
    Console.WriteLine("Invoice is compliant with SdI v1.9.1 rules.");
}
else
{
    Console.WriteLine($"Invoice has {validation.Errors.Count} error(s):");
    foreach (ValidationError error in validation.Errors)
    {
        Console.WriteLine($"  [Code: {error.Code}] Field: {error.PropertyName} - {error.Message}");
    }
}

// Check non-blocking warnings
foreach (ValidationError warning in validation.Warnings)
{
    Console.WriteLine($"  [Warning: {warning.Code}] Field: {warning.PropertyName} - {warning.Message}");
}
```

---

### Step 4: Exporting to XML & JSON

You can serialize any domain invoice back to XML compliant with official Agenzia delle Entrate schemas, or to JSON:

```csharp
using ElectronicInvoiceAdE.Extensions;

// Generate formatted XML string
string xmlContent = ordinary.ToXml(indented: true);

// Save directly to a file
ordinary.Save("output_invoice.xml", indented: true);

// Generate formatted JSON string
string jsonContent = ordinary.ToJson(indented: true);
```

---

### Step 5: Processing Batch ZIP Archives

When downloading invoices from provider portals or fiscal drawers, multiple invoices are frequently bundled into a `.zip` archive containing both `.xml` and `.xml.p7m` files:

```csharp
List<ZipInvoiceEntry> entries = ElectronicInvoiceDocument.LoadFromZip("monthly_invoices.zip");

Console.WriteLine($"Processed {entries.Count} invoices from archive.");

foreach (ZipInvoiceEntry entry in entries)
{
    Console.WriteLine($"Filename: {entry.EntryName} (Digital Signature: {entry.WasSigned})");

    if (entry.Invoice is OrdinaryInvoice invoice)
    {
        Console.WriteLine($"  Number: {invoice.PrimaryBody.GeneralData.DocumentGeneralData.Number}");
        Console.WriteLine($"  Supplier: {invoice.Header.Supplier.Identification.LegalName.CompanyName}");
    }
}
```

---

## 4. Next Steps

- Consult [System Architecture](architecture.md) to understand modular decoupling.
- Check [Domain Models & Lookups](domain-models-and-lookups.md) for ministerial lookup code catalogs.
- Review [Validation & Fiscal Rules](validation-and-fiscal-rules.md) for detailed business rules and error codes.
- Check [Database Persistence](database-persistence-efcore.md) for saving invoices with Entity Framework Core.
