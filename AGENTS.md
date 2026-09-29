# Repository Context — ElectronicInvoiceAdE

This document provides AI coding agents and contributors with high-signal context, conventions, commands, and rules for working in the **ElectronicInvoiceAdE** codebase safely and effectively.

---

## 1. Overview & Tech Stack

- **Target Frameworks**: Multi-targeting **.NET 8.0**, **.NET 9.0**, and **.NET 10.0** (`net8.0;net9.0;net10.0`).
- **Language Version**: **C# 13.0** (`<LangVersion>13.0</LangVersion>`), with `<Nullable>enable</Nullable>` and `<ImplicitUsings>enable</ImplicitUsings>`.
- **Solution File**: Modern XML solution format `ElectronicInvoiceAdE.slnx`.
- **Architecture**: Modular Clean Domain Library with Facade Pattern and zero circular dependencies:
  - **Facade**: `ElectronicInvoiceAdE` (umbrella metapackage, `ElectronicInvoiceDocument`, fluent extensions).
  - **Core**: `ElectronicInvoiceAdE.Core` (Ordinary/Simplified domain models, fiscal lookup catalogs).
  - **Serialization**: `ElectronicInvoiceAdE.Serialization` (reflection-based `XmlMapper`, `InvoiceReader`, `InvoiceWriter`, JSON).
  - **Validation**: `ElectronicInvoiceAdE.Validation` (`InvoiceValidator`, SdI error rules, Partita IVA Luhn, Codice Fiscale).
  - **Security**: `ElectronicInvoiceAdE.Security` (CAdES-BES PKCS#7 `.p7m` envelope unpacking, ZIP batch reader).
  - **Data**: `ElectronicInvoiceAdE.Data` (`ElectronicInvoiceDbContext`, EF Core relational persistence).
- **Central Management**:
  - `Directory.Build.props`: Global build settings, warnings-as-errors governance, SourceLink, and packaging metadata.
  - `Directory.Packages.props`: Central Package Management (CPM) pinning all library and test dependencies.
- **Versioning**: `MinVer` Git-tag versioning using milestone approach without `v` prefix (e.g. `1.9.1-preview.1`, `1.9.1`).
- **Publishing & CI/CD**: Automated GitHub Actions workflow (`.github/workflows/publish.yml`) publishing to NuGet.org upon pushing tags or creating releases.

---

## 2. Solution Structure

```
ElectronicInvoiceAdE.slnx
Directory.Build.props                         # Shared build governance, MinVer, SourceLink
Directory.Packages.props                      # Central Package Management (CPM)
LICENSE                                       # MIT License (Copyright (c) 2026 Ub4thaan)
README.md                                     # Main repository and facade package documentation
.github/workflows/publish.yml                 # Automated NuGet publishing CI/CD pipeline
/src
  ├── ElectronicInvoiceAdE/                   # High-level facade (ElectronicInvoiceDocument, InvoiceExtensions)
  │     ├── Extensions/                       # Extension methods (.Validate(), .ToXml(), .ToJson())
  │     └── ElectronicInvoiceAdE.csproj       # Facade metapackage referencing Core, Ser, Val, Sec
  ├── ElectronicInvoiceAdE.Core/              # Domain entities, lookup catalogs, ministerial codes
  │     ├── Common/                           # Shared interfaces and helpers
  │     ├── Lookups/                          # Catalogs: DocumentType, TaxRegime, VatNature, etc.
  │     ├── Ordinary/                         # Ordinary invoice models (Header, Body, Lines, Totals)
  │     ├── Simplified/                       # Simplified invoice models (FSM10)
  │     └── README.md                         # Dedicated Core package documentation
  ├── ElectronicInvoiceAdE.Serialization/     # High-performance XML and JSON serializers
  │     ├── Serialization/                    # XmlMapper, InvoiceReader, InvoiceWriter, Json
  │     └── README.md                         # Dedicated Serialization package documentation
  ├── ElectronicInvoiceAdE.Validation/        # Fiscal rules and checksum algorithms
  │     ├── Validation/                       # InvoiceValidator, SdI rules (00327, 00400, etc.), Tax algorithms
  │     └── README.md                         # Dedicated Validation package documentation
  ├── ElectronicInvoiceAdE.Security/          # Cryptography and archive processing
  │     ├── Security/                         # SignedInvoiceReader (PKCS#7 .p7m), ZipInvoiceReader
  │     └── README.md                         # Dedicated Security package documentation
  └── ElectronicInvoiceAdE.Data/              # Relational database persistence
        ├── Configurations/                   # EF Core EntityTypeConfigurations
        ├── Entities/                         # Queryable relational entities
        └── README.md                         # Dedicated Data package documentation
/tests
  ├── ElectronicInvoiceAdE.Tests/             # Facade tests, real-world sample XML scenarios
  ├── ElectronicInvoiceAdE.Core.Tests/        # Domain model behavior and catalog validity
  ├── ElectronicInvoiceAdE.Serialization.Tests/ # XML namespace resilience, codepage 1252, round-trips
  ├── ElectronicInvoiceAdE.Validation.Tests/  # SdI error checks, Partita IVA Luhn, Codice Fiscale omocodia
  └── ElectronicInvoiceAdE.Security.Tests/    # DER/PEM CAdES-BES signature unwrapping & cert extraction
/docs                                         # Deep architectural guides and documentation
```

---

## 3. Key CLI Commands

### Build & Restore
- **Restore dependencies**:
  ```shell
  dotnet restore
  ```
- **Build (Debug)**:
  ```shell
  dotnet build
  ```
- **Build (Release mode)**:
  ```shell
  dotnet build --configuration Release --no-restore
  ```

### Testing
- **Run all 456 tests across all target runtimes**:
  ```shell
  dotnet test --configuration Release --no-build
  ```
- **Run a specific test project**:
  ```shell
  dotnet test tests/ElectronicInvoiceAdE.Validation.Tests
  ```
- **Run targeted tests by filter**:
  ```shell
  dotnet test --filter "FullyQualifiedName~PartitaIva"
  dotnet test --filter "FullyQualifiedName~XmlMapper"
  ```

### Packaging & Code Hygiene
- **Create all 6 NuGet packages & symbol packages**:
  ```shell
  dotnet pack --configuration Release --output ./artifacts
  ```
- **Code formatting check / fix**:
  ```shell
  dotnet format --verify-no-changes
  dotnet format
  ```

---

## 4. C# & Engineering Conventions

### Compiler & Warning Governance
- `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` is strictly enforced. No warnings are permitted in production code.
- XML documentation warnings (`1591`, `1570`) are suppressed via `NoWarn` where appropriate.
- `<NuGetAudit>true</NuGetAudit>` with `<NuGetAuditLevel>low</NuGetAuditLevel>` is active. All dependencies must be free of reported security vulnerabilities.

### Language & Modern C# 13 Idioms
- Utilize modern C# 13 features: primary constructors, collection expressions `[...]`, pattern matching (`is`, `switch` expressions), and required properties.
- **Nullable Reference Types**: Active across all projects (`<Nullable>enable</Nullable>`). Avoid null-forgiving operators (`!`) unless rigorously proven unreachable.

### Architectural Rules
- **Zero Circular References**: The dependency graph strictly flows downwards:
  - `ElectronicInvoiceAdE` (Facade) $\rightarrow$ `Security`, `Validation`, `Serialization`, `Core`
  - `Security` $\rightarrow$ `Serialization`, `Core`
  - `Serialization` $\rightarrow$ `Core`
  - `Validation` $\rightarrow$ `Core`
  - `Data` $\rightarrow$ `Core`
  - `Core` has **zero project dependencies**.
- **Dual-Naming Model Discipline**:
  - Public C# domain models, properties, and methods use clear, idiomatic **English** (`Supplier`, `Customer`, `DocumentType`, `TotalPrice`, `LineItem`).
  - Underlying serialization mapping attributes bind 1-to-1 to official Italian ministerial XML nodes (`CedentePrestatore`, `CessionarioCommittente`, `TipoDocumento`, `PrezzoTotale`, `DettaglioLinee`).

---

## 5. Verification Protocol Before Task Completion

Before completing any task or reporting work done to the user, execute this mandatory 3-step verification checklist:

1. **Build Verification**:
   ```shell
   dotnet build --configuration Release
   ```
   *Must exit with code 0 and 0 errors/warnings.*

2. **Test Suite Verification**:
   ```shell
   dotnet test --configuration Release --no-build
   ```
   *All 456 unit and integration tests must pass.*

3. **Packaging Verification (if modifying project properties or assets)**:
   ```shell
   dotnet pack --configuration Release --no-build --output ./artifacts
   ```
   *All 6 `.nupkg` and `.snupkg` files must be created without packaging warnings.*
   *(Clean up `./artifacts` directory after verification).*

---

## 6. Documentation & README Synchronization Rule

Whenever introducing new features, domain properties, lookup codes, validation rules, or architectural modifications, **always keep the documentation in sync**:

1. **Root [`README.md`](README.md)**:
   - Update if public API facade methods (`ElectronicInvoiceDocument`, extension methods), target frameworks, or core features change.
2. **Sub-Package READMEs (`src/<Project>/README.md`)**:
   - Each of the 5 sub-packages has a dedicated `README.md` embedded into its NuGet package. Keep their feature lists and quick-start code snippets aligned with the sub-package's public API:
     - [`src/ElectronicInvoiceAdE.Core/README.md`](src/ElectronicInvoiceAdE.Core/README.md)
     - [`src/ElectronicInvoiceAdE.Serialization/README.md`](src/ElectronicInvoiceAdE.Serialization/README.md)
     - [`src/ElectronicInvoiceAdE.Validation/README.md`](src/ElectronicInvoiceAdE.Validation/README.md)
     - [`src/ElectronicInvoiceAdE.Security/README.md`](src/ElectronicInvoiceAdE.Security/README.md)
     - [`src/ElectronicInvoiceAdE.Data/README.md`](src/ElectronicInvoiceAdE.Data/README.md)
3. **Specialized Guides in [`docs/`](docs/)**:
   - Update the relevant specialized guide when modifying underlying subsystems:
     - [`docs/architecture.md`](docs/architecture.md): Layer interactions, project references, design patterns.
     - [`docs/domain-models-and-lookups.md`](docs/domain-models-and-lookups.md): New lookup codes, enum values, Ordinary/Simplified models.
     - [`docs/serialization-and-encodings.md`](docs/serialization-and-encodings.md): XML parsing heuristics, encodings, JSON options.
     - [`docs/validation-and-fiscal-rules.md`](docs/validation-and-fiscal-rules.md): Ministerial SdI error codes, Partita IVA, Codice Fiscale rules.
     - [`docs/security-p7m-and-zip.md`](docs/security-p7m-and-zip.md): PKCS#7 envelope processing, certificate extraction, ZIP formats.
     - [`docs/database-persistence-efcore.md`](docs/database-persistence-efcore.md): EF Core entity configurations, relational tables, indexing.
     - [`docs/next-steps-and-roadmap.md`](docs/next-steps-and-roadmap.md): Updating status of completed roadmap items.
