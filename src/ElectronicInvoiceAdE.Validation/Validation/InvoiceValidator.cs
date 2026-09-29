using System;
using System.Linq;
using ElectronicInvoiceAdE.Lookups;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Simplified;

namespace ElectronicInvoiceAdE.Validation;

/// <summary>
/// Offline validator executing official Agenzia delle Entrate Technical Specifications v1.9.1 controls and error codes.
/// </summary>
public static class InvoiceValidator
{
    public static ValidationResult Validate(OrdinaryInvoice invoice)
    {
        var result = new ValidationResult();

        // 1. Root and Transmission
        if (!TransmissionFormat.IsOrdinary(invoice.Version))
        {
            result.AddError("00428", $"Format {invoice.Version} is not a valid ordinary invoice version.", "Version");
        }

        var trans = invoice.Header.TransmissionData;
        if (string.IsNullOrWhiteSpace(trans.TransmissionSequence))
        {
            result.AddError("00427", "Transmission sequence (ProgressivoInvio) is required.", "Header.TransmissionData.TransmissionSequence");
        }
        else if (trans.TransmissionSequence.Length > 10)
        {
            result.AddError("00427", "Transmission sequence cannot exceed 10 characters.", "Header.TransmissionData.TransmissionSequence");
        }

        if (trans.Format == TransmissionFormat.PublicAdministration)
        {
            if (trans.RecipientCode?.Length != 6)
            {
                result.AddError("00427", "Recipient code (CodiceDestinatario) for Public Administration (FPA12) must be exactly 6 characters.", "Header.TransmissionData.RecipientCode");
            }
        }
        else if (trans.Format == TransmissionFormat.PrivateParties)
        {
            if (trans.RecipientCode?.Length != 7)
            {
                result.AddError("00427", "Recipient code (CodiceDestinatario) for private parties (FPR12) must be exactly 7 characters.", "Header.TransmissionData.RecipientCode");
            }
        }

        // Transmitter ID
        if (string.IsNullOrWhiteSpace(trans.TransmitterId.Code))
        {
            result.AddError("00300", "Transmitter code is required.", "Header.TransmissionData.TransmitterId.Code");
        }
        else if (trans.TransmitterId.CountryCode == "IT" && !ItalianTaxValidation.ValidateCodiceFiscale(trans.TransmitterId.Code))
        {
            result.AddWarning("00300", $"Transmitter code '{trans.TransmitterId.Code}' is not a valid Italian tax code.", "Header.TransmissionData.TransmitterId.Code");
        }

        // 2. Supplier (CedentePrestatore)
        var supplier = invoice.Header.Supplier;
        var supTax = supplier.Identification.TaxId;
        if (string.IsNullOrWhiteSpace(supTax.Code))
        {
            result.AddError("00300", "Supplier VAT identification code is required.", "Header.Supplier.Identification.TaxId.Code");
        }
        else if (supTax.Code.Length > 28)
        {
            result.AddError("00300", "Supplier VAT identification code cannot exceed 28 characters.", "Header.Supplier.Identification.TaxId.Code");
        }
        else if (supTax.CountryCode == "IT" && !ItalianTaxValidation.ValidatePartitaIva(supTax.Code))
        {
            result.AddWarning("00300", $"Supplier Partita IVA '{supTax.Code}' check digit is invalid.", "Header.Supplier.Identification.TaxId.Code");
        }

        // Rule 00327: VAT Group check
        if (supTax.CountryCode == "IT" && supTax.Code != null && supTax.Code.StartsWith("15") && supTax.Code.Length == 11)
        {
            var cf = supplier.Identification.FiscalCode;
            if (string.IsNullOrWhiteSpace(cf))
            {
                result.AddError("00327", "When supplier Partita IVA belongs to a VAT Group (starts with '15'), Fiscal Code (CodiceFiscale) is mandatory.", "Header.Supplier.Identification.FiscalCode");
            }
            else if (string.Equals(cf.Trim(), supTax.Code.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                result.AddError("00327", "When supplier Partita IVA belongs to a VAT Group, Fiscal Code must represent the participant entity and cannot be identical to the VAT Group ID.", "Header.Supplier.Identification.FiscalCode");
            }
        }

        if (string.IsNullOrWhiteSpace(supplier.Identification.LegalName.CompanyName) &&
            string.IsNullOrWhiteSpace(supplier.Identification.LegalName.FirstName) &&
            string.IsNullOrWhiteSpace(supplier.Identification.LegalName.LastName))
        {
            result.AddError("00417", "Supplier company name or person first and last name must be specified.", "Header.Supplier.Identification.LegalName");
        }

        if (!TaxRegime.IsValid(supplier.Identification.TaxRegime))
        {
            result.AddError("00405", $"Supplier Tax Regime '{supplier.Identification.TaxRegime}' is not valid.", "Header.Supplier.Identification.TaxRegime");
        }

        if (string.IsNullOrWhiteSpace(supplier.Address.Street) || string.IsNullOrWhiteSpace(supplier.Address.Municipality) || string.IsNullOrWhiteSpace(supplier.Address.PostalCode))
        {
            result.AddError("00417", "Supplier registered address (Street, PostalCode, Municipality) is mandatory.", "Header.Supplier.Address");
        }

        // 3. Customer (CessionarioCommittente)
        var customer = invoice.Header.Customer;
        var custTax = customer.Identification.TaxId;
        var custCf = customer.Identification.FiscalCode;

        if (string.IsNullOrWhiteSpace(custTax?.Code) && string.IsNullOrWhiteSpace(custCf))
        {
            result.AddError("00417", "Customer must have either a VAT Identification (IdFiscaleIVA) or Fiscal Code (CodiceFiscale).", "Header.Customer.Identification");
        }

        if (trans.RecipientCode == "XXXXXXX" && custTax?.CountryCode == "IT")
        {
            result.AddError("00313", "Recipient code 'XXXXXXX' is only allowed for non-resident entities (IdPaese must not be IT).", "Header.TransmissionData.RecipientCode");
        }

        if (custTax != null && custTax.CountryCode == "IT" && !string.IsNullOrWhiteSpace(custTax.Code) && !ItalianTaxValidation.ValidatePartitaIva(custTax.Code))
        {
            result.AddWarning("00300", $"Customer Partita IVA '{custTax.Code}' check digit is invalid.", "Header.Customer.Identification.TaxId.Code");
        }

        if (!string.IsNullOrWhiteSpace(custCf) && (custTax == null || custTax.CountryCode == "IT") && !ItalianTaxValidation.ValidateCodiceFiscale(custCf))
        {
            result.AddWarning("00305", $"Customer Codice Fiscale '{custCf}' check digit is invalid.", "Header.Customer.Identification.FiscalCode");
        }

        // 4. Bodies
        if (invoice.Bodies.Count == 0)
        {
            result.AddError("00417", "At least one invoice body (<FatturaElettronicaBody>) must be present.", "Bodies");
            return result;
        }

        for (var b = 0; b < invoice.Bodies.Count; b++)
        {
            var body = invoice.Bodies[b];
            var docData = body.GeneralData.DocumentGeneralData;

            if (!DocumentType.IsOrdinary(docData.DocumentType))
            {
                result.AddError("00405", $"Document type '{docData.DocumentType}' is not a valid ordinary invoice document type.", $"Bodies[{b}].GeneralData.DocumentGeneralData.DocumentType");
            }

            if (string.IsNullOrWhiteSpace(docData.Number))
            {
                result.AddError("00417", "Document number is mandatory.", $"Bodies[{b}].GeneralData.DocumentGeneralData.Number");
            }

            if (docData.Date == default)
            {
                result.AddError("00417", "Document date is mandatory.", $"Bodies[{b}].GeneralData.DocumentGeneralData.Date");
            }

            // Lines
            if (body.GoodsAndServices.Lines.Count == 0)
            {
                result.AddError("00417", "At least one line item (<DettaglioLinee>) must be present.", $"Bodies[{b}].GoodsAndServices.Lines");
            }

            for (var l = 0; l < body.GoodsAndServices.Lines.Count; l++)
            {
                var line = body.GoodsAndServices.Lines[l];
                if (string.IsNullOrWhiteSpace(line.Description))
                {
                    result.AddError("00417", "Line description cannot be empty.", $"Bodies[{b}].GoodsAndServices.Lines[{l}].Description");
                }
                else if (line.Description.Length > 1000)
                {
                    result.AddError("00417", "Line description cannot exceed 1000 characters.", $"Bodies[{b}].GoodsAndServices.Lines[{l}].Description");
                }

                if (line.VatRate == 0 && string.IsNullOrWhiteSpace(line.VatNature))
                {
                    result.AddError("00400", "VAT Nature (<Natura>) is mandatory when VAT rate is 0%.", $"Bodies[{b}].GoodsAndServices.Lines[{l}].VatNature");
                }
                else if (line.VatRate > 0 && !string.IsNullOrWhiteSpace(line.VatNature))
                {
                    result.AddError("00401", "VAT Nature (<Natura>) cannot be specified when VAT rate is greater than 0%.", $"Bodies[{b}].GoodsAndServices.Lines[{l}].VatNature");
                }
            }

            // Summaries
            if (body.GoodsAndServices.VatSummaries.Count == 0)
            {
                result.AddError("00417", "At least one VAT summary block (<DatiRiepilogo>) must be present.", $"Bodies[{b}].GoodsAndServices.VatSummaries");
            }
        }

        return result;
    }

    public static ValidationResult Validate(SimplifiedInvoice invoice)
    {
        var result = new ValidationResult();

        if (!TransmissionFormat.IsSimplified(invoice.Version))
        {
            result.AddError("00428", $"Format {invoice.Version} is not a valid simplified invoice version (must be FSM10).", "Version");
        }

        var sup = invoice.Header.Supplier;
        if (string.IsNullOrWhiteSpace(sup.TaxId.Code))
        {
            result.AddError("00300", "Supplier Tax ID code is required.", "Header.Supplier.TaxId.Code");
        }

        // Rule 00327: VAT Group check for Simplified invoice
        if (sup.TaxId.CountryCode == "IT" && sup.TaxId.Code != null && sup.TaxId.Code.StartsWith("15") && sup.TaxId.Code.Length == 11)
        {
            var cf = sup.FiscalCode;
            if (string.IsNullOrWhiteSpace(cf))
            {
                result.AddError("00327", "When supplier Partita IVA belongs to a VAT Group (starts with '15'), Fiscal Code (CodiceFiscale) is mandatory.", "Header.Supplier.FiscalCode");
            }
            else if (string.Equals(cf.Trim(), sup.TaxId.Code.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                result.AddError("00327", "When supplier Partita IVA belongs to a VAT Group, Fiscal Code must represent the participant entity and cannot be identical to the VAT Group ID.", "Header.Supplier.FiscalCode");
            }
        }

        var body = invoice.PrimaryBody;
        if (!DocumentType.IsSimplified(body.GeneralData.DocumentGeneralData.DocumentType))
        {
            result.AddError("00405", $"Document type '{body.GeneralData.DocumentGeneralData.DocumentType}' is not valid for a simplified invoice (expected TD07, TD08, or TD09).", "PrimaryBody.GeneralData.DocumentGeneralData.DocumentType");
        }

        if (body.GoodsAndServices.Count == 0)
        {
            result.AddError("00417", "At least one line item is required.", "PrimaryBody.GoodsAndServices");
        }

        return result;
    }
}
