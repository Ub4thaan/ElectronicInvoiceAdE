# ElectronicInvoiceAdE Documentation

Welcome to the comprehensive technical documentation for **ElectronicInvoiceAdE**, an open-source, high-performance .NET library for Italian Electronic Invoicing (**Fatturazione Elettronica**) supporting the technical specifications of the **Agenzia delle Entrate** and **Sistema di Interscambio (SdI)**.

---

## Documentation Index

| Guide | Description | Target Audience |
| :--- | :--- | :--- |
| [**Getting Started**](getting-started.md) | Quickstart guide, installation, basic invoice loading, offline validation, and serialization. | Developers, Integrators |
| [**Architecture & Design**](architecture.md) | Modular solution structure, layer responsibilities, design trade-offs, and data flow. | Architects, Senior Developers |
| [**Domain Models & Lookups**](domain-models-and-lookups.md) | Detailed mapping of `OrdinaryInvoice`, `SimplifiedInvoice`, and ministerial lookup tables (TD, RF, N, MP, TC, RT). | Domain Engineers, Tax Specialists |
| [**Serialization & Encodings**](serialization-and-encodings.md) | Deep dive into `XmlMapper`, prefix tolerance, legacy encoding handling (`windows-1252`), and JSON round-trip. | Backend Developers |
| [**Security: P7M & ZIP Processing**](security-p7m-and-zip.md) | CAdES-BES PKCS#7 envelope extraction, certificate metadata inspection, and ZIP batch processing. | Security Engineers, Integrators |
| [**Validation & Fiscal Rules**](validation-and-fiscal-rules.md) | Offline mathematical check-digit algorithms (Partita IVA Luhn, Codice Fiscale) and SdI v1.9.1 business rules. | Compliance Reviewers, QA |
| [**Database Persistence (EF Core)**](database-persistence-efcore.md) | `ElectronicInvoiceAdE.Data` relational mapping, flattened database entities, and generic `DbContext` usage. | Data Engineers, DBAs |
| [**Next Steps & Roadmap**](next-steps-and-roadmap.md) | Systematic backlog and future roadmap: SdI notifications (RC/NS/MC), active signing, HTML/PDF viewer, tax calculation engine. | Contributors, Product Leads |
| [**AI Usage Disclosure**](ai-usage-disclosure.md) | Transparent disclosure of artificial intelligence tools utilized during development, testing, and documentation. | Open-Source Community, Auditors |

---

## Role-Based Reading Paths

### 1. "I want to parse and validate an invoice in under 5 minutes"
1. Read the [Getting Started Guide](getting-started.md).
2. Review the code samples in [Root README](../README.md).
3. Check the [Validation & Fiscal Rules](validation-and-fiscal-rules.md) guide if handling validation error codes.

### 2. "I need to understand how the domain and XML mappings work"
1. Read [Domain Models & Lookups](domain-models-and-lookups.md) to understand the English-to-Italian mapping.
2. Read [Serialization & Encodings](serialization-and-encodings.md) to learn how `XmlMapper` resolves XML tags without brittle serialization attributes.

### 3. "I need to process signed .p7m invoices and batch ZIP files"
1. Review [Security: P7M & ZIP Processing](security-p7m-and-zip.md) for ASN.1 DER and Base64 PEM extraction details.
2. Learn how to extract cryptographic signature metadata (certificate subject, issuer, validity period).

### 4. "I want to contribute to future features"
1. Read the [Repository Context & Conventions](../AGENTS.md) for build, testing, and documentation synchronization rules.
2. Review [System Architecture](architecture.md) for clean layer separation rules.
3. Consult the prioritized backlog in [Next Steps & Roadmap](next-steps-and-roadmap.md).
4. Read the contribution standards in [AI Usage Disclosure](ai-usage-disclosure.md).
