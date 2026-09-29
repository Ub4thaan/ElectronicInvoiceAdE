using System;
using System.IO;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Security;
using Xunit;

namespace ElectronicInvoiceAdE.Security.Tests;

public class P7mTests
{
    [Fact]
    public void TestBinaryP7mUnpacking()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Samples", "IT02182030391_31.xml.p7m");
        Assert.True(File.Exists(path), $"File not found at {path}");

        var package = SignedInvoiceReader.Unpack(path);
        Assert.NotNull(package);
        Assert.True(package.XmlContent.Length > 0);
        Assert.True(package.Signatures.Count > 0);

        var signature = package.Signatures[0];
        Assert.False(string.IsNullOrEmpty(signature.Subject));
        Assert.False(string.IsNullOrEmpty(signature.Issuer));

        var invoice = package.ReadOrdinary();
        Assert.NotNull(invoice);
        Assert.Equal("FPR12", invoice.Version);
        Assert.Equal("31", invoice.Header.TransmissionData.TransmissionSequence);
        Assert.Equal("02182030391", invoice.Header.Supplier.Identification.TaxId.Code);
    }

    [Fact]
    public void TestBase64P7mUnpacking()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Samples", "IT02182030391_31.Base64.xml.p7m");
        Assert.True(File.Exists(path), $"File not found at {path}");

        var package = SignedInvoiceReader.Unpack(path);
        Assert.NotNull(package);
        Assert.True(package.XmlContent.Length > 0);
        Assert.True(package.Signatures.Count > 0);

        var invoice = package.ReadOrdinary();
        Assert.NotNull(invoice);
        Assert.Equal("FPR12", invoice.Version);
        Assert.Equal("31", invoice.Header.TransmissionData.TransmissionSequence);
        Assert.Equal("02182030391", invoice.Header.Supplier.Identification.TaxId.Code);
    }
}
