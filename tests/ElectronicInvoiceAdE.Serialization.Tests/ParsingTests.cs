using System;
using System.IO;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Serialization;
using Xunit;

namespace ElectronicInvoiceAdE.Serialization.Tests;

public class ParsingTests
{
    [Fact]
    public void TestInvoiceReaderOnFpr02()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Samples", "IT01234567890_FPR02.xml");
        Assert.True(File.Exists(path), $"File not found at {path}");

        var invoice = InvoiceReader.ReadOrdinary(path);

        Assert.NotNull(invoice);
        Assert.Equal("FPR12", invoice.Version);
        Assert.Equal("abc", invoice.IssuingSystem);
        Assert.Equal("00001", invoice.Header.TransmissionData.TransmissionSequence);
        Assert.Equal("FPR12", invoice.Header.TransmissionData.Format);
        Assert.Equal("0000000", invoice.Header.TransmissionData.RecipientCode);
        Assert.Equal("betagamma@pec.it", invoice.Header.TransmissionData.RecipientPec);
        Assert.Equal("IT", invoice.Header.TransmissionData.TransmitterId.CountryCode);
        Assert.Equal("01234567890", invoice.Header.TransmissionData.TransmitterId.Code);

        // Supplier
        Assert.Equal("IT", invoice.Header.Supplier.Identification.TaxId.CountryCode);
        Assert.Equal("01234567890", invoice.Header.Supplier.Identification.TaxId.Code);
        Assert.Equal("SOCIETA' ALPHA SRL", invoice.Header.Supplier.Identification.LegalName.CompanyName);
        Assert.Equal("RF01", invoice.Header.Supplier.Identification.TaxRegime);
        Assert.Equal("VIALE ROMA 543", invoice.Header.Supplier.Address.Street);
        Assert.Equal("07100", invoice.Header.Supplier.Address.PostalCode);
        Assert.Equal("SASSARI", invoice.Header.Supplier.Address.Municipality);

        // Customer
        Assert.Equal("09876543210", invoice.Header.Customer.Identification.FiscalCode);
        Assert.Equal("BETA GAMMA", invoice.Header.Customer.Identification.LegalName.CompanyName);

        // Body
        Assert.Single(invoice.Bodies);
        var body = invoice.PrimaryBody;
        Assert.Equal("TD01", body.GeneralData.DocumentGeneralData.DocumentType);
        Assert.Equal("EUR", body.GeneralData.DocumentGeneralData.Currency);
        Assert.Equal("123", body.GeneralData.DocumentGeneralData.Number);

        // Order Reference
        Assert.Single(body.GeneralData.PurchaseOrders);
        Assert.Equal("66685", body.GeneralData.PurchaseOrders[0].DocumentId);
        Assert.Equal("1", body.GeneralData.PurchaseOrders[0].ItemNumber);
        Assert.Single(body.GeneralData.PurchaseOrders[0].LineNumbers);
        Assert.Equal(1, body.GeneralData.PurchaseOrders[0].LineNumbers[0]);

        // Transport
        Assert.NotNull(body.GeneralData.TransportData);
        Assert.NotNull(body.GeneralData.TransportData.Carrier);
        Assert.Equal("Trasporto spa", body.GeneralData.TransportData.Carrier.LegalName.CompanyName);
        Assert.Equal("IT", body.GeneralData.TransportData.Carrier.TaxId.CountryCode);
        Assert.Equal("24681012141", body.GeneralData.TransportData.Carrier.TaxId.Code);
        Assert.Equal(new DateTime(2012, 10, 22, 16, 46, 12), body.GeneralData.TransportData.DeliveryDateTime);

        // Lines
        Assert.Equal(4, body.GoodsAndServices.Lines.Count);
        Assert.Equal(1, body.GoodsAndServices.Lines[0].LineNumber);
        Assert.Equal(1.00m, body.GoodsAndServices.Lines[0].UnitPrice);
        Assert.Equal(5.00m, body.GoodsAndServices.Lines[0].TotalPrice);
        Assert.Equal(22.00m, body.GoodsAndServices.Lines[0].VatRate);

        // Line 3 with discount amount
        var line3 = body.GoodsAndServices.Lines[2];
        Assert.Equal(3, line3.LineNumber);
        Assert.Single(line3.DiscountsOrSurcharges);
        Assert.Equal("SC", line3.DiscountsOrSurcharges[0].Type);
        Assert.Equal(-1.71m, line3.DiscountsOrSurcharges[0].Amount);

        // Line 4 with discount percentage
        var line4 = body.GoodsAndServices.Lines[3];
        Assert.Equal(4, line4.LineNumber);
        Assert.Single(line4.DiscountsOrSurcharges);
        Assert.Equal("SC", line4.DiscountsOrSurcharges[0].Type);
        Assert.Equal(10.0m, line4.DiscountsOrSurcharges[0].Percentage);

        // VAT Summary
        Assert.Single(body.GoodsAndServices.VatSummaries);
        var vat = body.GoodsAndServices.VatSummaries[0];
        Assert.Equal(22.00m, vat.VatRate);
        Assert.Equal(36.08m, vat.TaxableAmount);
        Assert.Equal(7.94m, vat.TaxAmount);
        Assert.Equal("D", vat.VatCollectibility);

        // Payment
        Assert.Single(body.Payments);
        Assert.Equal("TP01", body.Payments[0].PaymentTerms);
        Assert.Single(body.Payments[0].PaymentDetails);
        var pay = body.Payments[0].PaymentDetails[0];
        Assert.Equal("MP01", pay.PaymentMethod);
        Assert.Equal(new DateTime(2015, 1, 30), pay.DueDate);
        Assert.Equal(36.08m, pay.Amount);
    }

    [Fact]
    public void TestInvoiceReaderOnFpa02()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Samples", "IT01234567890_FPA02.xml");
        Assert.True(File.Exists(path), $"File not found at {path}");

        var invoice = InvoiceReader.ReadOrdinary(path);

        Assert.NotNull(invoice);
        Assert.Equal("FPA12", invoice.Version);
        Assert.Equal("00001", invoice.Header.TransmissionData.TransmissionSequence);
        Assert.Equal("FPA12", invoice.Header.TransmissionData.Format);
        Assert.Equal("AAAAAA", invoice.Header.TransmissionData.RecipientCode);
        Assert.Equal("SOCIETA' ALPHA SRL", invoice.Header.Supplier.Identification.LegalName.CompanyName);
        Assert.Equal("AMMINISTRAZIONE BETA", invoice.Header.Customer.Identification.LegalName.CompanyName);

        var body = invoice.PrimaryBody;
        Assert.Equal("TD01", body.GeneralData.DocumentGeneralData.DocumentType);
        Assert.Equal(4, body.GoodsAndServices.Lines.Count);
        Assert.Equal(1.00m, body.GoodsAndServices.Lines[0].UnitPrice);
        Assert.Equal(5.00m, body.GoodsAndServices.Lines[0].TotalPrice);
        Assert.Equal(2.00m, body.GoodsAndServices.Lines[1].UnitPrice);
        Assert.Equal(20.00m, body.GoodsAndServices.Lines[1].TotalPrice);
    }

    [Fact]
    public void TestInvoiceReaderOnSimplifiedInvoice()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Samples", "IT01234567890_FSM10.xml");
        Assert.True(File.Exists(path), $"File not found at {path}");

        var invoice = InvoiceReader.ReadSimplified(path);

        Assert.NotNull(invoice);
        Assert.Equal("FSM10", invoice.Version);
        Assert.Equal("abc", invoice.IssuingSystem);
        Assert.Equal("00001", invoice.Header.TransmissionData.TransmissionSequence);
        Assert.Equal("FSM10", invoice.Header.TransmissionData.Format);
        Assert.Equal("0000000", invoice.Header.TransmissionData.RecipientCode);
        Assert.Equal("betagamma@pec.it", invoice.Header.TransmissionData.RecipientPec);

        // Supplier
        Assert.Equal("IT", invoice.Header.Supplier.TaxId.CountryCode);
        Assert.Equal("01234567890", invoice.Header.Supplier.TaxId.Code);
        Assert.Equal("SOCIETA' ALPHA SRL", invoice.Header.Supplier.CompanyName);
        Assert.Equal("RF01", invoice.Header.Supplier.TaxRegime);
        Assert.Equal("VIALE ROMA 543", invoice.Header.Supplier.Address.Street);
        Assert.Equal("07100", invoice.Header.Supplier.Address.PostalCode);
        Assert.Equal("SASSARI", invoice.Header.Supplier.Address.Municipality);

        // Customer
        Assert.NotNull(invoice.Header.Customer.TaxIdentification);
        Assert.Equal("IT", invoice.Header.Customer.TaxIdentification.TaxId?.CountryCode);
        Assert.Equal("09876543210", invoice.Header.Customer.TaxIdentification.TaxId?.Code);
        Assert.NotNull(invoice.Header.Customer.OtherIdentification);
        Assert.Equal("BETA GAMMA", invoice.Header.Customer.OtherIdentification.CompanyName);
        Assert.Equal("VIA TORINO 38-B", invoice.Header.Customer.OtherIdentification.Address?.Street);

        // Body
        Assert.Single(invoice.Bodies);
        var body = invoice.PrimaryBody;
        Assert.Equal("TD07", body.GeneralData.DocumentGeneralData.DocumentType);
        Assert.Equal("EUR", body.GeneralData.DocumentGeneralData.Currency);
        Assert.Equal("123", body.GeneralData.DocumentGeneralData.Number);
        Assert.Equal(new DateTime(2019, 1, 1), body.GeneralData.DocumentGeneralData.Date);

        // Line
        Assert.Single(body.GoodsAndServices);
        Assert.Equal(25.00m, body.GoodsAndServices[0].Amount);
        Assert.NotNull(body.GoodsAndServices[0].VatData);
        Assert.Equal(22.00m, body.GoodsAndServices[0].VatData?.Rate);
    }

    [Fact]
    public void TestInvoiceReaderOnWindows1252Encoding()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Samples", "IT02182030391_32.windows-1252.xml");
        Assert.True(File.Exists(path), $"File not found at {path}");

        var invoice = InvoiceReader.ReadOrdinary(path);

        Assert.NotNull(invoice);
        Assert.Equal("FPR12", invoice.Version);
        Assert.Equal("32", invoice.Header.TransmissionData.TransmissionSequence);
        Assert.Equal("12345678901", invoice.Header.Supplier.Identification.TaxId.Code);
    }
}
