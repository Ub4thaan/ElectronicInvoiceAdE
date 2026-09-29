using System;

namespace ElectronicInvoiceAdE.Data.Entities;

/// <summary>
/// Represents a Simplified Invoice for database persistence.
/// </summary>
public class SimplifiedInvoiceEntity
{
    public virtual Guid Id { get; set; }
    
    public virtual Guid InvoiceFileId { get; set; }
    public virtual InvoiceFileEntity File { get; set; } = null!;

    public virtual string Version { get; set; } = string.Empty;
    public virtual string? IssuingSystem { get; set; }

    // Core flattened header properties
    public virtual string SupplierTaxId { get; set; } = string.Empty;
    public virtual string SupplierName { get; set; } = string.Empty;
    
    public virtual string CustomerTaxId { get; set; } = string.Empty;
    public virtual string CustomerName { get; set; } = string.Empty;

    // Simplified invoices often have a simpler body or fewer lines.
    // For this example, we keep it simple.
    public virtual string DocumentType { get; set; } = string.Empty;
    public virtual string DocumentNumber { get; set; } = string.Empty;
    public virtual DateTime DocumentDate { get; set; }
    public virtual decimal TotalAmount { get; set; }
    public virtual decimal TaxAmount { get; set; }
}
