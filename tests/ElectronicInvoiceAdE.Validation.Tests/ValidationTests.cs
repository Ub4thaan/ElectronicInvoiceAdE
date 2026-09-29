using System;
using System.IO;
using System.Linq;
using ElectronicInvoiceAdE.Extensions;
using ElectronicInvoiceAdE.Lookups;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Serialization;
using ElectronicInvoiceAdE.Simplified;
using ElectronicInvoiceAdE.Validation;
using Xunit;

namespace ElectronicInvoiceAdE.Validation.Tests;

public class ValidationTests
{
    [Fact]
    public void TestValidOfficialInvoicePassesValidation()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Samples", "IT01234567890_FPR02.xml");
        var invoice = InvoiceReader.ReadOrdinary(path);

        var result = invoice.Validate();
        Assert.True(result.IsValid, $"Validation failed: {string.Join(", ", result.Errors)}");
    }

    [Fact]
    public void TestPartitaIvaValidationAlgorithm()
    {
        // Valid authentic Italian VAT numbers
        Assert.True(ItalianTaxValidation.ValidatePartitaIva("02182030391")); // Invoicetronic
        Assert.True(ItalianTaxValidation.ValidatePartitaIva("00488410010")); // Telecom Italia
        Assert.True(ItalianTaxValidation.ValidatePartitaIva("00811720580")); // Enel
        Assert.True(ItalianTaxValidation.ValidatePartitaIva("01114601006")); // Poste Italiane

        // Invalid Italian VAT numbers (tampered check digit, dummy sequences, or wrong length)
        Assert.False(ItalianTaxValidation.ValidatePartitaIva("01234567890")); // Expected check digit is 7, dummy uses 0
        Assert.False(ItalianTaxValidation.ValidatePartitaIva("02182030392")); // Tampered
        Assert.False(ItalianTaxValidation.ValidatePartitaIva("12345"));
        Assert.False(ItalianTaxValidation.ValidatePartitaIva("ABCDEFGHIJK"));
    }

    [Fact]
    public void TestCodiceFiscaleValidationAlgorithm()
    {
        // Valid Italian 16-character fiscal codes with calculated check letters
        Assert.True(ItalianTaxValidation.ValidateCodiceFiscale("RSSMRA85M01H501Q")); // Mario Rossi, check letter Q
        Assert.True(ItalianTaxValidation.ValidateCodiceFiscale("VRDGPP80A01F205X")); // Giuseppe Verdi, check letter X

        // Valid 11-digit legal entity fiscal code
        Assert.True(ItalianTaxValidation.ValidateCodiceFiscale("02182030391"));

        // Invalid fiscal codes
        Assert.False(ItalianTaxValidation.ValidateCodiceFiscale("RSSMRA85M01H501A")); // Wrong check letter
        Assert.False(ItalianTaxValidation.ValidateCodiceFiscale("SHORT"));
    }

    [Fact]
    public void TestZeroVatRateRequiresVatNature()
    {
        var invoice = OrdinaryInvoice.Create();
        invoice.Header.TransmissionData.TransmissionSequence = "00001";
        invoice.Header.Supplier.Identification.TaxId.Code = "02182030391";
        invoice.Header.Supplier.Identification.LegalName.CompanyName = "Test SRL";
        invoice.Header.Supplier.Address.Street = "Via Roma 1";
        invoice.Header.Supplier.Address.PostalCode = "00100";
        invoice.Header.Supplier.Address.Municipality = "Roma";
        invoice.Header.Customer.Identification.FiscalCode = "RSSMRA85M01H501Q";

        var body = invoice.PrimaryBody;
        body.GeneralData.DocumentGeneralData.Number = "1";
        body.GeneralData.DocumentGeneralData.Date = DateTime.Today;

        // Add line with 0% VAT but NO nature
        body.GoodsAndServices.Lines.Add(new()
        {
            LineNumber = 1,
            Description = "Consulting exempt",
            UnitPrice = 100m,
            TotalPrice = 100m,
            VatRate = 0m,
            VatNature = null // Missing!
        });

        body.GoodsAndServices.VatSummaries.Add(new()
        {
            VatRate = 0m,
            TaxableAmount = 100m,
            TaxAmount = 0m
        });

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.True(result.Errors.Any(e => e.ErrorCode == "00400"), "Expected error 00400 for missing VatNature");
    }

    [Fact]
    public void TestPositiveVatRateCannotHaveVatNature()
    {
        var invoice = OrdinaryInvoice.Create();
        invoice.Header.TransmissionData.TransmissionSequence = "00001";
        invoice.Header.Supplier.Identification.TaxId.Code = "02182030391";
        invoice.Header.Supplier.Identification.LegalName.CompanyName = "Test SRL";
        invoice.Header.Supplier.Address.Street = "Via Roma 1";
        invoice.Header.Supplier.Address.PostalCode = "00100";
        invoice.Header.Supplier.Address.Municipality = "Roma";
        invoice.Header.Customer.Identification.FiscalCode = "RSSMRA85M01H501Q";

        var body = invoice.PrimaryBody;
        body.GeneralData.DocumentGeneralData.Number = "1";
        body.GeneralData.DocumentGeneralData.Date = DateTime.Today;

        // Add line with 22% VAT and a VatNature (forbidden!)
        body.GoodsAndServices.Lines.Add(new()
        {
            LineNumber = 1,
            Description = "Goods",
            UnitPrice = 100m,
            TotalPrice = 100m,
            VatRate = 22m,
            VatNature = VatNature.ExcludedArt15 // Forbidden with VAT > 0!
        });

        body.GoodsAndServices.VatSummaries.Add(new()
        {
            VatRate = 22m,
            TaxableAmount = 100m,
            TaxAmount = 22m
        });

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.True(result.Errors.Any(e => e.ErrorCode == "00401"), "Expected error 00401 for VatNature with VatRate > 0");
    }

    [Fact]
    public void TestRecipientCodeLengthMismatch()
    {
        var invoice = OrdinaryInvoice.Create(TransmissionFormat.PublicAdministration);
        invoice.Header.TransmissionData.TransmissionSequence = "00001";
        invoice.Header.TransmissionData.RecipientCode = "1234567"; // 7 chars, but FPA12 requires 6!

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.True(result.Errors.Any(e => e.ErrorCode == "00427"), "Expected error 00427 for FPA recipient code length");
    }

    #region Helper Factories

    private static OrdinaryInvoice CreateValidOrdinaryInvoice()
    {
        var invoice = OrdinaryInvoice.Create(TransmissionFormat.PrivateParties);
        invoice.Header.TransmissionData.TransmissionSequence = "00001";
        invoice.Header.TransmissionData.RecipientCode = "0000000";
        invoice.Header.TransmissionData.TransmitterId.CountryCode = "IT";
        invoice.Header.TransmissionData.TransmitterId.Code = "02182030391";

        invoice.Header.Supplier.Identification.TaxId.CountryCode = "IT";
        invoice.Header.Supplier.Identification.TaxId.Code = "02182030391";
        invoice.Header.Supplier.Identification.LegalName.CompanyName = "Test SRL";
        invoice.Header.Supplier.Identification.TaxRegime = TaxRegime.Ordinary;
        invoice.Header.Supplier.Address.Street = "Via Roma 1";
        invoice.Header.Supplier.Address.PostalCode = "00100";
        invoice.Header.Supplier.Address.Municipality = "Roma";

        invoice.Header.Customer.Identification.FiscalCode = "RSSMRA85M01H501Q";

        var body = invoice.PrimaryBody;
        body.GeneralData.DocumentGeneralData.DocumentType = DocumentType.Invoice;
        body.GeneralData.DocumentGeneralData.Number = "1";
        body.GeneralData.DocumentGeneralData.Date = DateTime.Today;

        body.GoodsAndServices.Lines.Add(new()
        {
            LineNumber = 1,
            Description = "Goods",
            UnitPrice = 100m,
            TotalPrice = 100m,
            VatRate = 22m
        });

        body.GoodsAndServices.VatSummaries.Add(new()
        {
            VatRate = 22m,
            TaxableAmount = 100m,
            TaxAmount = 22m
        });

        return invoice;
    }

    private static SimplifiedInvoice CreateValidSimplifiedInvoice()
    {
        var invoice = new SimplifiedInvoice();
        invoice.Header.TransmissionData.TransmissionSequence = "00001";
        invoice.Header.TransmissionData.RecipientCode = "0000000";
        invoice.Header.Supplier.TaxId.CountryCode = "IT";
        invoice.Header.Supplier.TaxId.Code = "02182030391";

        var body = invoice.PrimaryBody;
        body.GeneralData.DocumentGeneralData.DocumentType = DocumentType.SimplifiedInvoice;
        body.GeneralData.DocumentGeneralData.Number = "1";
        body.GeneralData.DocumentGeneralData.Date = DateTime.Today;

        body.GoodsAndServices.Add(new()
        {
            Description = "Item 1",
            Amount = 50m
        });

        return invoice;
    }

    #endregion

    #region Rule 00327 Tests (VAT Group Fiscal Code)

    [Fact]
    public void TestVatGroupRequiresFiscalCode()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Header.Supplier.Identification.TaxId.Code = "15123456780";
        invoice.Header.Supplier.Identification.FiscalCode = null;

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00327");
    }

    [Fact]
    public void TestVatGroupFiscalCodeCannotMatchVatId()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Header.Supplier.Identification.TaxId.Code = "15123456780";
        invoice.Header.Supplier.Identification.FiscalCode = "15123456780";

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00327");
    }

    [Fact]
    public void TestVatGroupWithValidParticipantFiscalCodePasses()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Header.Supplier.Identification.TaxId.Code = "15123456780";
        invoice.Header.Supplier.Identification.FiscalCode = "02182030391";

        var result = invoice.Validate();
        Assert.DoesNotContain(result.Errors, e => e.ErrorCode == "00327");
    }

    [Fact]
    public void TestSimplifiedInvoiceVatGroupRule00327()
    {
        var invoice = CreateValidSimplifiedInvoice();
        invoice.Header.Supplier.TaxId.Code = "15123456780";
        invoice.Header.Supplier.FiscalCode = null;

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00327");
    }

    #endregion

    #region Rule 00428 Tests (Version / Format)

    [Fact]
    public void TestInvalidOrdinaryInvoiceVersionThrows00428()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Version = "FSM10";

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00428");
    }

    [Fact]
    public void TestInvalidSimplifiedInvoiceVersionThrows00428()
    {
        var invoice = CreateValidSimplifiedInvoice();
        invoice.Version = "FPR12";

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00428");
    }

    #endregion

    #region Rule 00427 Tests (Transmission Data & Recipient Codes)

    [Fact]
    public void TestMissingTransmissionSequenceThrows00427()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Header.TransmissionData.TransmissionSequence = "";

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00427");
    }

    [Fact]
    public void TestTransmissionSequenceExceeding10CharsThrows00427()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Header.TransmissionData.TransmissionSequence = "12345678901";

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00427");
    }

    [Fact]
    public void TestFprRecipientCodeLengthMismatchThrows00427()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Header.TransmissionData.RecipientCode = "123456"; // 6 chars instead of 7 for FPR12

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00427");
    }

    #endregion

    #region Rule 00313 Tests (Foreign Recipient Code XXXXXXX)

    [Fact]
    public void TestForeignRecipientCodeWithItalianCustomerThrows00313()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Header.TransmissionData.RecipientCode = "XXXXXXX";
        invoice.Header.Customer.Identification.TaxId = new Common.TaxId { CountryCode = "IT", Code = "02182030391" };

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00313");
    }

    [Fact]
    public void TestForeignRecipientCodeWithNonItalianCustomerPasses()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Header.TransmissionData.RecipientCode = "XXXXXXX";
        invoice.Header.Customer.Identification.TaxId = new Common.TaxId { CountryCode = "FR", Code = "12345678901" };

        var result = invoice.Validate();
        Assert.DoesNotContain(result.Errors, e => e.ErrorCode == "00313");
    }

    #endregion

    #region Rule 00405 Tests (Tax Regime & Document Types)

    [Fact]
    public void TestInvalidTaxRegimeThrows00405()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Header.Supplier.Identification.TaxRegime = "RF99";

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00405");
    }

    [Fact]
    public void TestInvalidOrdinaryDocumentTypeThrows00405()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.PrimaryBody.GeneralData.DocumentGeneralData.DocumentType = "TD07"; // Simplified doc type

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00405");
    }

    [Fact]
    public void TestInvalidSimplifiedDocumentTypeThrows00405()
    {
        var invoice = CreateValidSimplifiedInvoice();
        invoice.PrimaryBody.GeneralData.DocumentGeneralData.DocumentType = "TD01"; // Ordinary doc type

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00405");
    }

    #endregion

    #region Rule 00417 Tests (Mandatory Document Data)

    [Fact]
    public void TestMissingSupplierNameThrows00417()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Header.Supplier.Identification.LegalName.CompanyName = "";
        invoice.Header.Supplier.Identification.LegalName.FirstName = null;
        invoice.Header.Supplier.Identification.LegalName.LastName = null;

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00417");
    }

    [Fact]
    public void TestMissingSupplierAddressThrows00417()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Header.Supplier.Address.Street = "";

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00417");
    }

    [Fact]
    public void TestCustomerMissingBothVatAndFiscalCodeThrows00417()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.Header.Customer.Identification.FiscalCode = null;
        invoice.Header.Customer.Identification.TaxId = null;

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00417");
    }

    [Fact]
    public void TestMissingDocumentNumberThrows00417()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.PrimaryBody.GeneralData.DocumentGeneralData.Number = "";

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00417");
    }

    [Fact]
    public void TestEmptyLineItemsThrows00417()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.PrimaryBody.GoodsAndServices.Lines.Clear();

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00417");
    }

    [Fact]
    public void TestEmptyLineDescriptionThrows00417()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.PrimaryBody.GoodsAndServices.Lines[0].Description = "";

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00417");
    }

    [Fact]
    public void TestMissingVatSummaryThrows00417()
    {
        var invoice = CreateValidOrdinaryInvoice();
        invoice.PrimaryBody.GoodsAndServices.VatSummaries.Clear();

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00417");
    }

    #endregion

    #region Simplified Invoice Tests

    [Fact]
    public void TestValidSimplifiedInvoicePassesValidation()
    {
        var invoice = CreateValidSimplifiedInvoice();
        var result = invoice.Validate();
        Assert.True(result.IsValid, $"Validation failed: {string.Join(", ", result.Errors)}");
    }

    [Fact]
    public void TestSimplifiedInvoiceMissingSupplierTaxIdThrows00300()
    {
        var invoice = CreateValidSimplifiedInvoice();
        invoice.Header.Supplier.TaxId.Code = "";

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00300");
    }

    [Fact]
    public void TestSimplifiedInvoiceEmptyGoodsAndServicesThrows00417()
    {
        var invoice = CreateValidSimplifiedInvoice();
        invoice.PrimaryBody.GoodsAndServices.Clear();

        var result = invoice.Validate();
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorCode == "00417");
    }

    #endregion
}
