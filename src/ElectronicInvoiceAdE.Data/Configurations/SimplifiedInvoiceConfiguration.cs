using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ElectronicInvoiceAdE.Data.Entities;

namespace ElectronicInvoiceAdE.Data.Configurations;

public class SimplifiedInvoiceConfiguration : IEntityTypeConfiguration<SimplifiedInvoiceEntity>
{
    public void Configure(EntityTypeBuilder<SimplifiedInvoiceEntity> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
