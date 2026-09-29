# AI Usage Disclosure & Technical Governance

This document provides a transparent, comprehensive disclosure regarding the use of Artificial Intelligence (AI) technologies throughout the lifecycle of the **ElectronicInvoiceAdE** project.

---

## 1. Commitment to Transparency

We believe in complete transparency and intellectual honesty regarding modern software development methodologies. **ElectronicInvoiceAdE** was engineered through a collaborative pair-programming paradigm combining senior human software engineering with advanced agentic AI capabilities provided by **Google DeepMind Antigravity** and **Gemini** models.

The goal of this disclosure is to delineate where AI was utilized, what methodologies were enforced to guarantee technical accuracy, and how human oversight governed the fiscal compliance and architectural integrity of the project.

---

## 2. Scope of AI Involvement

Artificial intelligence tools were utilized across several phases of the engineering lifecycle:

### A. Architectural Scaffolding & Decoupling
- **Module Separation**: Designing clean boundaries across the solution (`Core`, `Serialization`, `Validation`, `Security`, `Data`, and the root facade `ElectronicInvoiceAdE`), guaranteeing zero circular dependencies.
- **Multi-Targeting Configuration**: Establishing MSBuild governance across `.NET 8.0, .NET 9.0, and .NET 10.0` with Central Package Management (`Directory.Packages.props`) and strict warning governance (`<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`).

### B. Domain Modeling & XML Schema Cross-Referencing
- **Tag Translation**: Mapping hundreds of abbreviated Italian tax tags (e.g. `CedentePrestatore`, `CessionarioCommittente`, `DatiBeniServizi`, `DatiRiepilogo`, `FormatoTrasmissione`) to strongly typed, idiomatic C# domain models in English (`Supplier`, `Customer`, `GoodsAndServices`, `VatSummaries`, `TransmissionFormat`).
- **Fiscal Lookup Catalogs**: Transcribing official technical lookup tables (`DocumentType` TD01–TD29, `TaxRegime` RF01–RF20, `VatNature` N1–N7, `PaymentMethod` MP01–MP23, `PensionFundType` TC01–TC22, and `WithholdingType` RT01–RT06) into typed constants with validation predicates.

### C. Algorithmic Translation of Fiscal Checksums
- **Partita IVA Validation**: Translating the official Italian modified Luhn algorithm (evaluating 11 numeric digits, odd/even weightings, and provincial office ranges) into pure, performant C# code.
- **Codice Fiscale Validation**: Implementing the 16-character alphanumeric checksum algorithm, including non-linear odd-position character conversion tables (`OddConversion`) and omocodia substitution character sets.
- **SdI Business Rules**: Codifying Agenzia delle Entrate Technical Specifications v1.9.1 rules, such as Rule `00327` (VAT Group mandatory participant check) and Rules `00400`/`00401` (0% VAT rate correlation with mandatory `Natura` codes).

### D. Comprehensive Test Suite Generation
- **Synthetic Test Scenarios**: Authoring edge cases, negative tests, and boundary value tests across all modules.
- **Format Variations**: Constructing tests for arbitrary XML namespace prefixes (`p:`, `b:`, default), empty element tags, legacy encodings (`windows-1252`), and PKCS#7 envelope variants (DER binary vs. Base64 PEM).
- The resulting automated test suite contains **456 tests**, all executing and passing across .NET 8.0, 9.0, and 10.0.

### E. Documentation & Visual Artifacts
- Authoring the root `README.md`, technical architecture guides, domain model documentation, and future roadmap.
- Generating Mermaid diagrams illustrating system architecture, data flows, and relational database schemas.

---

## 3. Human Governance & Quality Assurance

While AI models served as powerful accelerators, the correctness, security, and viability of the codebase are anchored in rigorous human-directed engineering:

1. **Deterministic Verification via Automated Tests**:
   - No code or rule generated with AI assistance is committed without deterministic automated unit test coverage.
   - All 456 tests execute natively in CI/CD across all target frameworks.
2. **Strict Compiler Governance**:
   - The solution enforces `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`. Any unhandled nullability warning, compiler deprecation, or lint issue blocks the build.
3. **Regulatory Cross-Auditing**:
   - All validation error codes (e.g. `00300`, `00313`, `00327`, `00400`, `00401`, `00427`, `00428`) are checked directly against official Italian ministerial decrees, technical specifications (v1.9.1), and sample XML documents released by the Agenzia delle Entrate.
4. **Zero Proprietary Data in Prompts**:
   - No sensitive, commercial, or confidential taxpayer data was used in prompts or training contexts. All test files utilize public sample data or synthetic tax codes.

---

## 4. Guidelines for Future AI-Assisted Contributions

Community contributors utilizing AI coding assistants (such as Copilot, Claude Code, ChatGPT, Gemini, or Antigravity) are warmly welcomed, provided they adhere to the following standards:

1. **Disclose AI Assistance**: Mention in the PR description if significant portions of the contribution were generated with AI assistance.
2. **Provide Verifiable Tests**: Every PR must include unit tests verifying the introduced behavior or bug fix.
3. **Verify Fiscal Accuracy**: If modifying fiscal algorithms (`ItalianTaxValidation`) or business rules (`InvoiceValidator`), cite the exact section and version of the Agenzia delle Entrate Technical Specifications being addressed.
4. **Adhere to Code Standards**: Code must compile without warnings on all three target frameworks (`net8.0`, `net9.0`, `net10.0`).
