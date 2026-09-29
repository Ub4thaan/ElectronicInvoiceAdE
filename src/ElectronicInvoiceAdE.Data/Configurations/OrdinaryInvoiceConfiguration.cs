using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ElectronicInvoiceAdE.Data.Entities;

namespace ElectronicInvoiceAdE.Data.Configurations;

public class OrdinaryInvoiceConfiguration : IEntityTypeConfiguration<OrdinaryInvoiceEntity>
{
    public void Configure(EntityTypeBuilder<OrdinaryInvoiceEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasMany(x => x.Bodies)
               .WithOne(x => x.OrdinaryInvoice)
               .HasForeignKey(x => x.OrdinaryInvoiceId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
