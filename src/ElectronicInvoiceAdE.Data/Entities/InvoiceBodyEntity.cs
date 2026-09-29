using System;
using System.Collections.Generic;

namespace ElectronicInvoiceAdE.Data.Entities;

public class InvoiceBodyEntity
{
    public virtual Guid Id { get; set; }

    public virtual Guid OrdinaryInvoiceId { get; set; }
    public virtual OrdinaryInvoiceEntity OrdinaryInvoice { get; set; } = null!;

    public virtual string DocumentType { get; set; } = string.Empty;
    public virtual string DocumentNumber { get; set; } = string.Empty;
    public virtual DateTime DocumentDate { get; set; }
    public virtual decimal? TotalAmount { get; set; }

    public virtual ICollection<InvoiceLineItemEntity> LineItems { get; set; } = new List<InvoiceLineItemEntity>();
}
