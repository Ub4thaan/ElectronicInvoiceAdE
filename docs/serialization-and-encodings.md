# Serialization & Encoding Handling

This guide details the serialization architecture in `ElectronicInvoiceAdE.Serialization`, explaining how `XmlMapper`, `InvoiceReader`, `InvoiceWriter`, and `JsonSerialization` handle real-world XML anomalies, arbitrary prefixes, and legacy codepages.

---

## 1. The Challenges of Italian E-Invoice XML

In production environments, electronic invoices originate from hundreds of different accounting and ERP systems spanning decades of software development. As a result, XML invoices frequently deviate from textbook formatting:

1. **Inconsistent Namespace Prefixes**: Invoices arrive prefixed with `p:FatturaElettronica`, `b:FatturaElettronica`, custom prefixes (`ns2:`, `fe:`), or no prefix at all (default XML namespace).
2. **Empty & Self-Closing Tags**: Tags like `<DatiAnagrafici/>`, `<ScontoMaggiorazione/>`, or empty strings appear inside optional elements.
3. **Legacy Single-Byte Encodings**: While the SdI technical specification requests UTF-8, many legacy applications declare and encode files in `windows-1252`, `ISO-8859-1`, or `ISO-8859-15` without converting accented Italian characters (`à, è, é, ì, ò, ù`).
4. **Number & Date Formatting**: Italian decimal representations (`12,50` vs `12.50`) and various date/time formats.

---

## 2. Reflection XML Mapping (`XmlMapper`)

Standard `System.Xml.Serialization.XmlSerializer` instances are strictly bound to declared XML namespaces and prefixes. Any slight prefix mismatch or unexpected XML structure causes deserialization failure.

`ElectronicInvoiceAdE` utilizes `XmlMapper`, an internal engine built on top of LINQ to XML (`System.Xml.Linq.XDocument`):

### How `XmlMapper` Deserializes
- **Local Name Resolution**: Elements are matched by their local XML tag name (`element.Name.LocalName`), completely ignoring arbitrary namespace prefixes (`p:`, `b:`, or empty).
- **Attribute Matching**: Resolves both attributes (`[XmlAttribute("versione")]`) and sub-elements (`[XmlElement("FatturaElettronicaHeader")]`).
- **Dynamic Conversion**: Automatically parses numbers, dates (`yyyy-MM-dd`), and strings using invariant culture while trimming whitespace.
- **Collection Support**: Transparently binds repeating elements (such as `<DettaglioLinee>`, `<DatiRiepilogo>`, `<DatiPagamento>`) to generic `List<T>` properties.

```csharp
using System.Xml.Linq;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Serialization;

XDocument doc = XDocument.Load("invoice.xml");
OrdinaryInvoice invoice = XmlMapper.Deserialize<OrdinaryInvoice>(doc.Root!);
```

---

## 3. Universal Reader (`InvoiceReader`)

`InvoiceReader` serves as the high-level parser for disk files, streams, and raw strings.

### Automatic Codepage Registration

By default, modern .NET does not include legacy codepages like Windows-1252. `InvoiceReader` registers the provider during static initialization:

```csharp
static InvoiceReader()
{
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
}
```

This guarantees that files with headers like:
```xml
<?xml version="1.0" encoding="windows-1252"?>
```
or
```xml
<?xml version="1.0" encoding="ISO-8859-1"?>
```
are decoded correctly without throwing `NotSupportedException: No data is available for encoding 1252`.

### Format Auto-Detection

When parsing without an explicit type, `InvoiceReader.Read(...)` examines the root element and version attribute to return either an `OrdinaryInvoice` or `SimplifiedInvoice`:

```csharp
using ElectronicInvoiceAdE.Serialization;

// Auto-detect format from file
object invoice = InvoiceReader.Read("unknown_invoice.xml");

// Or parse explicitly
OrdinaryInvoice ordinary = InvoiceReader.ReadOrdinary("ordinary.xml");
SimplifiedInvoice simplified = InvoiceReader.ReadSimplified("simplified.xml");
```

---

## 4. Writing Compliant XML (`InvoiceWriter`)

`InvoiceWriter` converts strongly typed domain instances into official, well-formatted XML conforming to Agenzia delle Entrate specifications:

- Automatically applies the standard namespace:
  - `http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v1.2` for Ordinary Invoices.
  - `http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v1.0` for Simplified Invoices.
- Formats dates as ISO `yyyy-MM-dd`.
- Controls output indentation:

```csharp
using ElectronicInvoiceAdE.Serialization;

// Serialize to an indented XML string
string xmlString = InvoiceWriter.ToXml(ordinaryInvoice, indented: true);

// Write directly to a file
InvoiceWriter.Write(ordinaryInvoice, "output.xml", indented: true);

// Write directly to a stream
using var memoryStream = new MemoryStream();
InvoiceWriter.Write(ordinaryInvoice, memoryStream, indented: false);
```

---

## 5. Structured JSON Serialization (`JsonSerialization`)

For REST APIs, webhooks, or messaging queues (e.g. RabbitMQ, Kafka), `ElectronicInvoiceAdE.Serialization` provides round-trip JSON serialization using `System.Text.Json`:

```csharp
using ElectronicInvoiceAdE.Serialization;

// Convert domain invoice to formatted JSON
string json = JsonSerialization.ToJson(ordinaryInvoice, indented: true);

// Restore invoice from JSON
OrdinaryInvoice restored = JsonSerialization.FromJson<OrdinaryInvoice>(json);
```
