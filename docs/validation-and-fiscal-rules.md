# Validation & Fiscal Rules

This guide details the offline validation engine provided by `ElectronicInvoiceAdE.Validation`, covering both mathematical check-digit algorithms and official business rules from **Agenzia delle Entrate Technical Specifications v1.9.1**.

---

## 1. Mathematical Checksum Algorithms (`ItalianTaxValidation`)

The `ItalianTaxValidation` class contains pure C# algorithmic implementations of Italian tax identity checks.

### Partita IVA (Italian VAT ID) Checksum
An Italian Partita IVA consists of **11 numeric digits**:
- Digits 1–7: Progressive company identification number.
- Digits 8–10: Revenue Agency Provincial Office identifier.
- Digit 11: Control check digit calculated using a modified Luhn formula.

```csharp
using ElectronicInvoiceAdE.Validation;

bool isValid = ItalianTaxValidation.ValidatePartitaIva("01234567890");
```

**Algorithm Details**:
1. Digits at odd positions (1st, 3rd, 5th, 7th, 9th) are summed directly.
2. Digits at even positions (2nd, 4th, 6th, 8th, 10th) are multiplied by 2; if the product exceeds 9, 9 is subtracted from it before adding to the sum.
3. The expected check digit is `(10 - (sum % 10)) % 10`.

### Codice Fiscale (Italian Fiscal Code) Checksum
Italian fiscal codes exist in two distinct formats:
1. **Legal Entities (11 digits)**: Evaluated using the Partita IVA algorithm above.
2. **Natural Persons (16 alphanumeric characters)**: Evaluated using character conversion weight tables and omocodia character replacement:

```csharp
bool isValid = ItalianTaxValidation.ValidateCodiceFiscale("RSSMRA85M01H501Z");
```

**Algorithm Details**:
- Characters 1–15 are evaluated.
- Odd positions (1st, 3rd, etc.) use a specialized non-linear conversion table (where `A=1`, `B=0`, `C=5`, `D=7`, etc.).
- Even positions (2nd, 4th, etc.) convert numeric digits directly and letters `A=0` through `Z=25`.
- The sum modulo 26 yields the 16th character (`A` through `Z`).

---

## 2. SdI Business Rules (`InvoiceValidator`)

The `InvoiceValidator` class evaluates an entire `OrdinaryInvoice` or `SimplifiedInvoice` against official SdI controls:

```csharp
using ElectronicInvoiceAdE.Validation;

ValidationResult result = InvoiceValidator.Validate(ordinaryInvoice);

if (!result.IsValid)
{
    foreach (ValidationError error in result.Errors)
    {
        Console.WriteLine($"[SdI {error.Code}] {error.Message} (Field: {error.PropertyName})");
    }
}
```

### Supported SdI Error Codes

| SdI Code | Description | Rule Details |
| :---: | :--- | :--- |
| **`00300`** | Invalid/Missing Tax ID | Validates presence and check digits for supplier Partita IVA and transmitter tax code. |
| **`00305`** | Invalid Customer Fiscal Code | Generates a warning if customer Codice Fiscale fails the checksum algorithm. |
| **`00313`** | Incompatible Recipient Code | Recipient code `XXXXXXX` is permitted only for non-resident entities (`IdPaese != IT`). |
| **`00327`** | Mandatory Participant CF for VAT Groups | When the supplier's Partita IVA belongs to a VAT Group (starts with `15` and has length 11), an individual participant `CodiceFiscale` is mandatory and cannot be identical to the VAT Group ID. |
| **`00400`** | Missing VAT Nature with 0% Rate | In line items (`DettaglioLinee`), when `AliquotaIVA == 0.00%`, a valid `Natura` code (e.g. `N2.2`, `N4`) is strictly mandatory. |
| **`00401`** | VAT Nature Present with Non-Zero Rate | In line items, when `AliquotaIVA > 0.00%`, specifying a `Natura` code is prohibited by SdI. |
| **`00405`** | Invalid Document Type / Tax Regime | Enforces standard `DocumentType` (TD01–TD29) and `TaxRegime` (RF01–RF20) codes. |
| **`00417`** | Mandatory Fields Missing | Verifies presence of document number, emission date, supplier address, and at least one line item. |
| **`00427`** | Transmission Sequence / Recipient Code Length | Recipient code must be 6 characters for `FPA12` (PA) and 7 characters for `FPR12` (private). Sequence max 10 chars. |
| **`00428`** | Invalid Transmission Version | Ordinary version must be `FPR12` or `FPA12`; simplified must be `FSM10`. |

---

## 3. Errors vs. Warnings

`ValidationResult` separates feedback into two severity levels:
- **`result.Errors`**: Fatal validation violations. If any error is present, `result.IsValid == false`. The invoice will be rejected by SdI with a *Notifica di Scarto (NS)*.
- **`result.Warnings`**: Non-blocking advisories (e.g. non-Italian tax code patterns or minor discrepancies that SdI might allow under certain circumstances). Warnings do not cause `result.IsValid` to be `false`.
