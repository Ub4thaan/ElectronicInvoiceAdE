using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ElectronicInvoiceAdE.Data.Entities;

namespace ElectronicInvoiceAdE.Data.Configurations;

public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItemEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceLineItemEntity> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
