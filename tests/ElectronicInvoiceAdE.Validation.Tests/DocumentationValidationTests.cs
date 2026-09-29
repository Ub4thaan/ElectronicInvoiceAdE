using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ElectronicInvoiceAdE.Serialization;
using ElectronicInvoiceAdE.Validation;
using ElectronicInvoiceAdE.Extensions;
using Xunit;

namespace ElectronicInvoiceAdE.Validation.Tests;

public class DocumentationValidationTests
{
    public static IEnumerable<object[]> GetExampleFiles()
    {
        var docsPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "documentation");
        if (!Directory.Exists(docsPath))
        {
            docsPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "documentation");
        }
        
        if (Directory.Exists(docsPath))
        {
            var files = Directory.GetFiles(docsPath, "*.xml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                yield return new object[] { Path.GetFileName(file), file };
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetExampleFiles))]
    public void CanValidateDocumentationExample(string fileName, string filePath)
    {
        Assert.True(File.Exists(filePath), $"File not found: {fileName} ({filePath})");
        
        var xml = File.ReadAllText(filePath);
        
        if (xml.Contains("FatturaElettronicaSemplificata"))
        {
            var invoice = InvoiceReader.ReadSimplifiedXml(xml);
            var result = invoice.Validate();
            // We just ensure it runs validation without throwing exceptions
            // We cannot assert result.IsValid because some examples might have deliberate errors
            Assert.NotNull(result);
        }
        else if (xml.Contains("FatturaElettronica"))
        {
            var invoice = InvoiceReader.ReadOrdinaryXml(xml);
            var result = invoice.Validate();
            Assert.NotNull(result);
        }
    }
}
