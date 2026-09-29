using System;

namespace ElectronicInvoiceAdE.Data.Entities;

/// <summary>
/// Represents the flattened core data of an Ordinary Invoice for database persistence.
/// </summary>
public class OrdinaryInvoiceEntity
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

    public virtual ICollection<InvoiceBodyEntity> Bodies { get; set; } = new List<InvoiceBodyEntity>();
}
