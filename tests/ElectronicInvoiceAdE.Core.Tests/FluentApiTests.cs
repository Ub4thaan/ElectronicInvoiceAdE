using System;
using System.IO;
using ElectronicInvoiceAdE.Extensions;
using ElectronicInvoiceAdE.Lookups;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Serialization;
using Xunit;

namespace ElectronicInvoiceAdE.Core.Tests;

public class FluentApiTests
{
    [Fact]
    public void TestElectronicInvoiceDocumentLoad()
    {
        var fprPath = Path.Combine(AppContext.BaseDirectory, "Samples", "IT01234567890_FPR02.xml");
        var obj1 = ElectronicInvoiceDocument.Load(fprPath);
        Assert.IsType<OrdinaryInvoice>(obj1);

        var fsmPath = Path.Combine(AppContext.BaseDirectory, "Samples", "IT01234567890_FSM10.xml");
        var obj2 = ElectronicInvoiceDocument.Load(fsmPath);
        Assert.IsType<Simplified.SimplifiedInvoice>(obj2);

        var p7mPath = Path.Combine(AppContext.BaseDirectory, "Samples", "IT02182030391_31.xml.p7m");
        var obj3 = ElectronicInvoiceDocument.Load(p7mPath);
        Assert.IsType<OrdinaryInvoice>(obj3);
    }

    [Fact]
    public void TestJsonSerializationRoundTrip()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Samples", "IT01234567890_FPR02.xml");
        var original = InvoiceReader.ReadOrdinary(path);

        var json = original.ToJson();
        Assert.False(string.IsNullOrEmpty(json));
        Assert.True(json.Contains("\"version\": \"FPR12\"") || json.Contains("\"version\":\"FPR12\""));

        var restored = JsonSerialization.FromOrdinaryJson(json);
        Assert.NotNull(restored);
        Assert.Equal(original.Version, restored.Version);
        Assert.Equal(original.Header.Supplier.Identification.LegalName.CompanyName, restored.Header.Supplier.Identification.LegalName.CompanyName);
        Assert.Equal(original.PrimaryBody.GoodsAndServices.Lines.Count, restored.PrimaryBody.GoodsAndServices.Lines.Count);
    }

    [Fact]
    public void TestLookupCodesCompleteness()
    {
        // DocumentType
        Assert.True(DocumentType.IsValid(DocumentType.Invoice));
        Assert.True(DocumentType.IsValid(DocumentType.CreditNote));
        Assert.True(DocumentType.IsValid(DocumentType.PurchaseFromSanMarinoWithVat)); // TD28
        Assert.True(DocumentType.IsValid(DocumentType.OmittedOrIrregularInvoicingCommunication)); // TD29
        Assert.Equal("TD01", DocumentType.Invoice);
        Assert.NotNull(DocumentType.TryGet("TD28")?.EnglishDescription);

        // TaxRegime
        Assert.True(TaxRegime.IsValid(TaxRegime.FlatRate)); // RF19
        Assert.True(TaxRegime.IsValid(TaxRegime.CrossBorderVatExemption)); // RF20
        Assert.NotNull(TaxRegime.TryGet("RF20")?.EnglishDescription);

        // VatNature
        Assert.True(VatNature.IsValid(VatNature.ExcludedArt15));
        Assert.True(VatNature.IsValid(VatNature.NotSubjectArticles7To7Septies)); // N2.1
        Assert.True(VatNature.IsValid(VatNature.ReverseChargeScrap)); // N6.1
        Assert.NotNull(VatNature.TryGet("N6.1")?.EnglishDescription);

        // PaymentMethod
        Assert.True(PaymentMethod.IsValid(PaymentMethod.BankTransfer)); // MP05
        Assert.True(PaymentMethod.IsValid(PaymentMethod.PagoPA)); // MP23
        Assert.NotNull(PaymentMethod.TryGet("MP23")?.EnglishDescription);

        // PensionFundType
        Assert.True(PensionFundType.IsValid(PensionFundType.Inps)); // TC22
        Assert.True(PensionFundType.IsValid(PensionFundType.Enasarco)); // TC07
        Assert.NotNull(PensionFundType.TryGet("TC22")?.EnglishDescription);
    }

    [Fact]
    public void TestLoadFromZipArchive()
    {
        var fprPath = Path.Combine(AppContext.BaseDirectory, "Samples", "IT01234567890_FPR02.xml");
        var p7mPath = Path.Combine(AppContext.BaseDirectory, "Samples", "IT02182030391_31.xml.p7m");

        var zipPath = Path.Combine(AppContext.BaseDirectory, "test_invoices.zip");
        if (File.Exists(zipPath)) File.Delete(zipPath);

        using (var zipStream = File.Create(zipPath))
        using (var archive = new System.IO.Compression.ZipArchive(zipStream, System.IO.Compression.ZipArchiveMode.Create))
        {
            var e1 = archive.CreateEntry("invoice1.xml");
            using (var es = e1.Open())
            using (var fs = File.OpenRead(fprPath))
                fs.CopyTo(es);

            var e2 = archive.CreateEntry("invoice2.xml.p7m");
            using (var es = e2.Open())
            using (var fs = File.OpenRead(p7mPath))
                fs.CopyTo(es);
        }

        var results = ElectronicInvoiceDocument.LoadFromZip(zipPath);
        Assert.Equal(2, results.Count);

        var first = results.Find(r => r.EntryName == "invoice1.xml");
        Assert.NotNull(first);
        Assert.False(first.WasSigned);
        Assert.IsType<OrdinaryInvoice>(first.Invoice);

        var second = results.Find(r => r.EntryName == "invoice2.xml.p7m");
        Assert.NotNull(second);
        Assert.True(second.WasSigned);
        Assert.IsType<OrdinaryInvoice>(second.Invoice);

        if (File.Exists(zipPath)) File.Delete(zipPath);
    }
}
