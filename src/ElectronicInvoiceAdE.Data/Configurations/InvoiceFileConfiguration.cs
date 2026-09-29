using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ElectronicInvoiceAdE.Data.Entities;

namespace ElectronicInvoiceAdE.Data.Configurations;

public class InvoiceFileConfiguration : IEntityTypeConfiguration<InvoiceFileEntity>
{
    public void Configure(EntityTypeBuilder<InvoiceFileEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.FileHash).IsUnique();

        builder.HasMany(x => x.OrdinaryInvoices)
               .WithOne(x => x.File)
               .HasForeignKey(x => x.InvoiceFileId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SimplifiedInvoices)
               .WithOne(x => x.File)
               .HasForeignKey(x => x.InvoiceFileId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
