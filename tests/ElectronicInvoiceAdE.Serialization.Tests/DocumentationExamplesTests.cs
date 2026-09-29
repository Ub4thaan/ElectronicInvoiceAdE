using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ElectronicInvoiceAdE.Serialization;
using Xunit;

namespace ElectronicInvoiceAdE.Serialization.Tests;

public class DocumentationExamplesTests
{
    public static IEnumerable<object[]> GetExampleFiles()
    {
        var docsPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "documentation");
        if (!Directory.Exists(docsPath))
        {
            // Try another relative path depending on where it runs from
            docsPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "documentation");
        }
        
        if (Directory.Exists(docsPath))
        {
            var files = Directory.GetFiles(docsPath, "*.xml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                // Return just filename and full path for xunit display
                yield return new object[] { Path.GetFileName(file), file };
            }
        }
    }

    [Theory]
    [MemberData(nameof(GetExampleFiles))]
    public void CanReadDocumentationExample(string fileName, string filePath)
    {
        Assert.True(File.Exists(filePath), $"File not found: {fileName} ({filePath})");
        
        // Read file contents
        var xml = File.ReadAllText(filePath);
        
        // Try reading it. We don't know if it's ordinary or simplified in advance,
        // so we check the root element to decide.
        if (xml.Contains("FatturaElettronicaSemplificata"))
        {
            var invoice = InvoiceReader.ReadSimplifiedXml(xml);
            Assert.NotNull(invoice);
        }
        else if (xml.Contains("FatturaElettronica"))
        {
            var invoice = InvoiceReader.ReadOrdinaryXml(xml);
            Assert.NotNull(invoice);
        }
        else 
        {
            // Some examples might be just fragments or different types (like messages)
            // If it's not a FatturaElettronica, we just pass
        }
    }
}
