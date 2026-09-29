using System;

namespace ElectronicInvoiceAdE.Data.Entities;

public class InvoiceLineItemEntity
{
    public virtual Guid Id { get; set; }

    public virtual Guid InvoiceBodyId { get; set; }
    public virtual InvoiceBodyEntity InvoiceBody { get; set; } = null!;

    public virtual int LineNumber { get; set; }
    public virtual string Description { get; set; } = string.Empty;
    public virtual decimal Quantity { get; set; }
    public virtual decimal UnitPrice { get; set; }
    public virtual decimal TotalPrice { get; set; }
    public virtual decimal VatRate { get; set; }
    public virtual string? VatNature { get; set; }
}
