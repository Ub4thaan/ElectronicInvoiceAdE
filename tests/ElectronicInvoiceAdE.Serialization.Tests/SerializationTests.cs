using System;
using System.IO;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Serialization;
using ElectronicInvoiceAdE.Simplified;
using Xunit;

namespace ElectronicInvoiceAdE.Serialization.Tests;

public class SerializationTests
{
    [Fact]
    public void TestRoundTripOrdinaryInvoiceFpr02()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Samples", "IT01234567890_FPR02.xml");
        var original = InvoiceReader.ReadOrdinary(path);

        // Serialize to XML string
        var xml = InvoiceWriter.ToXml(original);
        Assert.True(xml.Contains("<p:FatturaElettronica"), "Root element with namespace prefix 'p' expected");
        Assert.True(xml.Contains("versione=\"FPR12\""), "versione attribute expected");
        Assert.True(xml.Contains("xmlns:p=\"http://ivaservizi.agenziaentrate.gov.it/docs/xsd/fatture/v1.2\""), "Official namespace expected");

        // Deserialize back from XML string
        var roundtripped = InvoiceReader.ReadOrdinaryXml(xml);

        Assert.Equal(original.Version, roundtripped.Version);
        Assert.Equal(original.Header.TransmissionData.TransmissionSequence, roundtripped.Header.TransmissionData.TransmissionSequence);
        Assert.Equal(original.Header.Supplier.Identification.TaxId.Code, roundtripped.Header.Supplier.Identification.TaxId.Code);
        Assert.Equal(original.Header.Supplier.Identification.LegalName.CompanyName, roundtripped.Header.Supplier.Identification.LegalName.CompanyName);
        Assert.Equal(original.PrimaryBody.GeneralData.DocumentGeneralData.Number, roundtripped.PrimaryBody.GeneralData.DocumentGeneralData.Number);
        Assert.Equal(original.PrimaryBody.GoodsAndServices.Lines.Count, roundtripped.PrimaryBody.GoodsAndServices.Lines.Count);
        Assert.Equal(original.PrimaryBody.GoodsAndServices.Lines[0].TotalPrice, roundtripped.PrimaryBody.GoodsAndServices.Lines[0].TotalPrice);
    }

    [Fact]
    public void TestRoundTripSimplifiedInvoice()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Samples", "IT01234567890_FSM10.xml");
        var original = InvoiceReader.ReadSimplified(path);

        // Serialize to XML string
        var xml = InvoiceWriter.ToXml(original);
        Assert.True(xml.Contains("<p:FatturaElettronicaSemplificata"), "Root element with namespace prefix 'p' expected");
        Assert.True(xml.Contains("versione=\"FSM10\""), "versione attribute expected");

        // Deserialize back from XML string
        var roundtripped = InvoiceReader.ReadSimplifiedXml(xml);

        Assert.Equal(original.Version, roundtripped.Version);
        Assert.Equal(original.Header.Supplier.TaxId.Code, roundtripped.Header.Supplier.TaxId.Code);
        Assert.Equal(original.PrimaryBody.GeneralData.DocumentGeneralData.Number, roundtripped.PrimaryBody.GeneralData.DocumentGeneralData.Number);
        Assert.Equal(original.PrimaryBody.GoodsAndServices.Count, roundtripped.PrimaryBody.GoodsAndServices.Count);
        Assert.Equal(original.PrimaryBody.GoodsAndServices[0].Amount, roundtripped.PrimaryBody.GoodsAndServices[0].Amount);
    }
}
