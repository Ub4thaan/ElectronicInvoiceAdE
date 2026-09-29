# Domain Models & Fiscal Lookup Catalogs

This document provides a detailed overview of the domain hierarchy in `ElectronicInvoiceAdE.Core` and the translation between English domain property names and official Italian Agenzia delle Entrate XML tags.

---

## 1. Domain Object Hierarchy

The library models both **Ordinary Invoices** (`OrdinaryInvoice`, format `FPR12` / `FPA12`) and **Simplified Invoices** (`SimplifiedInvoice`, format `FSM10`).

```
OrdinaryInvoice (FatturaElettronica)
├── Header (FatturaElettronicaHeader)
│   ├── TransmissionData (DatiTrasmissione)
│   ├── Supplier (CedentePrestatore)
│   │   ├── Identification (DatiAnagrafici)
│   │   ├── Address (Sede)
│   │   ├── PermanentEstablishment (StabileOrganizzazione)
│   │   ├── ReaRegistration (IscrizioneREA)
│   │   └── ContactData (Contatti)
│   ├── Customer (CessionarioCommittente)
│   │   ├── Identification (DatiAnagrafici)
│   │   ├── Address (Sede)
│   │   └── PermanentEstablishment (StabileOrganizzazione)
│   └── ThirdPartyEmitter (TerzoIntermediarioOSoggettoEmittente)
└── Bodies[] (FatturaElettronicaBody)
    ├── GeneralData (DatiGenerali)
    │   ├── DocumentGeneralData (DatiGeneraliDocumento)
    │   ├── OrderPurchaseData[] (DatiOrdineAcquisto)
    │   ├── ContractData[] (DatiContratto)
    │   ├── TransportData (DatiTrasporto)
    │   └── MainInvoiceData (DatiFatturaPrincipale)
    ├── GoodsAndServices (DatiBeniServizi)
    │   ├── Lines[] (DettaglioLinee)
    │   └── VatSummaries[] (DatiRiepilogo)
    ├── Vehicles (DatiVeicoli)
    ├── PaymentData[] (DatiPagamento)
    │   └── PaymentDetails[] (DettaglioPagamento)
    └── Attachments[] (Allegati)
```

---

## 2. English to Italian XML Tag Mapping

Properties in `ElectronicInvoiceAdE.Core` use clean, idiomatic English names annotated with `[XmlElement]` and `[XmlAttribute]` attributes referencing official ministerial XML tags.

### Header Mapping

| C# Property | XML Tag | Description |
| :--- | :--- | :--- |
| `TransmissionData.Format` | `FormatoTrasmissione` | Transmission format (`FPR12` for B2B/B2C, `FPA12` for PA). |
| `TransmissionData.TransmissionSequence` | `ProgressivoInvio` | Unique transmission sequence counter (max 10 chars). |
| `TransmissionData.RecipientCode` | `CodiceDestinatario` | Channel code (6 chars for PA, 7 chars for private, `XXXXXXX` for foreign). |
| `TransmissionData.RecipientPec` | `PECDestinatario` | Destination Certified Email address (optional). |
| `Supplier.Identification.TaxId` | `IdFiscaleIVA` | Supplier VAT ID (`IdPaese` + `IdCodice`). |
| `Supplier.Identification.FiscalCode` | `CodiceFiscale` | Supplier Italian Tax Code (mandatory for VAT groups). |
| `Supplier.Identification.LegalName` | `Anagrafica` | Company name (`Denominazione`) or Person name (`Nome`/`Cognome`). |
| `Supplier.Identification.TaxRegime` | `RegimeFiscale` | Tax regime code (e.g. `RF01`, `RF19`). |
| `Supplier.Address` | `Sede` | Street, Postal Code, Municipality, Province, Nation. |
| `Customer.Identification` | `CessionarioCommittente/DatiAnagrafici` | Customer tax identifier and legal name. |

### Document General Data Mapping

| C# Property | XML Tag | Description |
| :--- | :--- | :--- |
| `DocumentGeneralData.DocumentType` | `TipoDocumento` | Document type code (e.g. `TD01`, `TD04`, `TD24`). |
| `DocumentGeneralData.Currency` | `Divisa` | Currency ISO code (`EUR`, `USD`, etc.). |
| `DocumentGeneralData.Date` | `Data` | Document emission date (`yyyy-MM-dd`). |
| `DocumentGeneralData.Number` | `Numero` | Invoice alphanumeric number. |
| `DocumentGeneralData.TotalAmount` | `ImportoTotaleDocumento` | Total invoice amount including taxes. |
| `DocumentGeneralData.Rounding` | `Arrotondamento` | Document rounding adjustment amount. |
| `DocumentGeneralData.StampDuty` | `DatiBollo` | Virtual stamp duty (`BolloVirtuale = SI`, `ImportoBollo`). |
| `DocumentGeneralData.WithholdingTax` | `DatiRitenuta` | Withholding tax (`TipoRitenuta`, `ImportoRitenuta`, `AliquotaRitenuta`, `CausalePagamento`). |

### Goods, Services & VAT Summaries Mapping

| C# Property | XML Tag | Description |
| :--- | :--- | :--- |
| `GoodsAndServices.Lines` | `DettaglioLinee` | Collection of individual invoice line items. |
| `InvoiceLineItem.LineNumber` | `NumeroLinea` | Sequential 1-based line number (1 to 9999). |
| `InvoiceLineItem.Description` | `Descrizione` | Description of goods or services. |
| `InvoiceLineItem.Quantity` | `Quantita` | Quantity delivered. |
| `InvoiceLineItem.UnitPrice` | `PrezzoUnitario` | Price per unit (up to 8 decimal places). |
| `InvoiceLineItem.TotalPrice` | `PrezzoTotale` | Total line amount after line discounts/surcharges. |
| `InvoiceLineItem.VatRate` | `AliquotaIVA` | Applicable VAT percentage rate (e.g. 22.00, 10.00, 4.00, 0.00). |
| `InvoiceLineItem.VatNature` | `Natura` | VAT nature code when VAT rate is 0% (e.g. `N2.2`, `N4`). |
| `GoodsAndServices.VatSummaries` | `DatiRiepilogo` | Grouped VAT summaries per rate and nature. |
| `VatSummary.TaxableAmount` | `ImponibileImporto` | Total taxable amount for this rate/nature. |
| `VatSummary.TaxAmount` | `Imposta` | Total VAT tax calculated for this rate/nature. |
| `VatSummary.VatCollectibility` | `EsigibilitaIVA` | Immediate (`I`), Deferred (`D`), or Split Payment (`S`). |

---

## 3. Strongly Typed Fiscal Lookups

The `ElectronicInvoiceAdE.Core.Lookups` namespace provides exhaustive typed constants and helper methods for official SdI code sets:

### 1. Document Types (`DocumentType`)

```csharp
using ElectronicInvoiceAdE.Lookups;

// Standard Ordinary Invoice types
string td01 = DocumentType.Invoice;                // "TD01" - Fattura
string td02 = DocumentType.AdvanceInvoice;         // "TD02" - Acconto / anticipo su fattura
string td04 = DocumentType.CreditNote;             // "TD04" - Nota di credito
string td05 = DocumentType.DebitNote;              // "TD05" - Nota di debito
string td06 = DocumentType.Fee;                    // "TD06" - Parcella

// Deferred & Self-invoicing (Autofatture / Integrazioni)
string td24 = DocumentType.DeferredInvoice;        // "TD24" - Fattura differita
string td16 = DocumentType.InternalReverseCharge;  // "TD16" - Integrazione fattura reverse charge interno
string td17 = DocumentType.ForeignServicesSelfInvoice; // "TD17" - Integrazione/autofattura acquisto servizi estero
string td28 = DocumentType.SanMarinoPurchasesWithVat;  // "TD28" - Acquisti da San Marino con IVA
string td29 = DocumentType.MissingInvoiceNotice;       // "TD29" - Comunicazione omessa/errata fattura

// Helper inspection
bool isOrd = DocumentType.IsOrdinary(docType);     // Checks if code is valid TD01..TD29
bool isSim = DocumentType.IsSimplified(docType);   // Checks if code is TD07, TD08, or TD09
```

### 2. Tax Regimes (`TaxRegime`)

```csharp
string rf01 = TaxRegime.Ordinary;                  // "RF01" - Regime ordinario
string rf02 = TaxRegime.Minimi;                    // "RF02" - Regime dei contribuenti minimi
string rf19 = TaxRegime.Forfettario;               // "RF19" - Regime forfettario
string rf20 = TaxRegime.CrossBorderExemption;      // "RF20" - Franchigia per transfrontalieri

bool isValid = TaxRegime.IsValid(regimeCode);      // Checks if regime is RF01..RF20
```

### 3. VAT Natures (`VatNature`)

Used when VAT rate is `0.00%` (required by rules `00400` / `00401`):

```csharp
// Excluded from VAT
string n1  = VatNature.ExcludedArt15;              // "N1" - Escluse ex art. 15 DPR 633/72

// Non-subject to VAT
string n21 = VatNature.NonSubjectWithoutRequisites; // "N2.1" - Non soggette ad IVA - mancanza requisiti
string n22 = VatNature.NonSubjectOther;             // "N2.2" - Non soggette - altri casi

// Non-taxable operations
string n31 = VatNature.NonTaxableExports;           // "N3.1" - Esportazioni
string n32 = VatNature.NonTaxableIntraCommunity;    // "N3.2" - Cessioni intracomunitarie
string n35 = VatNature.NonTaxableDeclarationsOfIntent; // "N3.5" - Dichiarazioni d'intento

// Exempt operations
string n4  = VatNature.Exempt;                     // "N4" - Esenti ex art. 10 DPR 633/72

// Reverse Charge
string n61 = VatNature.ReverseChargeScrap;         // "N6.1" - Cessione di rottami
string n63 = VatNature.ReverseChargeSubcontractingConstruction; // "N6.3" - Subappalto edilizia
string n67 = VatNature.ReverseChargeElectronics;   // "N6.7" - Cessione telefoni cellulari/PC
```

### 4. Payment Methods (`PaymentMethod`)

```csharp
string mp01 = PaymentMethod.Cash;                  // "MP01" - Contanti
string mp02 = PaymentMethod.Check;                 // "MP02" - Assegno
string mp05 = PaymentMethod.BankTransfer;          // "MP05" - Bonifico bancario
string mp08 = PaymentMethod.CreditCard;            // "MP08" - Carta di pagamento
string mp12 = PaymentMethod.DirectDebitSepa;       // "MP12" - RID / SEPA Direct Debit
string mp23 = PaymentMethod.PagoPA;                // "MP23" - PagoPA
```

---

## 4. Simplified Invoices (`SimplifiedInvoice`)

The simplified invoice format (`FSM10`) is used for transactions with total amounts not exceeding €400,00 or for credit/debit rectification notes referencing previous invoices:

- Modeled in `ElectronicInvoiceAdE.Simplified.SimplifiedInvoice`.
- Features simplified supplier and customer identification blocks.
- Line items specify a total gross amount (`Importo`) and optionally VAT rate (`Aliquota`) or tax amount (`Imposta`).
- Support for rectification data (`RectifiedInvoiceData` / `DatiFatturaRettificata`).
