using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ElectronicInvoiceAdE.Data.Entities;

namespace ElectronicInvoiceAdE.Data.Configurations;

public class InvoiceBodyConfiguration : IEntityTypeConfiguration<InvoiceBodyEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceBodyEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasMany(x => x.LineItems)
               .WithOne(x => x.InvoiceBody)
               .HasForeignKey(x => x.InvoiceBodyId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
