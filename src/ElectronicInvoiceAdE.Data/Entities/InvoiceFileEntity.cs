using System;
using System.Collections.Generic;

namespace ElectronicInvoiceAdE.Data.Entities;

/// <summary>
/// Represents the physical XML file containing one or more invoices.
/// </summary>
public class InvoiceFileEntity
{
    public virtual Guid Id { get; set; }
    
    public virtual string RelativePath { get; set; } = string.Empty;
    
    public virtual string FileHash { get; set; } = string.Empty;
    
    public virtual byte[]? XmlContent { get; set; }

    public virtual ICollection<OrdinaryInvoiceEntity> OrdinaryInvoices { get; set; } = new List<OrdinaryInvoiceEntity>();
    public virtual ICollection<SimplifiedInvoiceEntity> SimplifiedInvoices { get; set; } = new List<SimplifiedInvoiceEntity>();
}
