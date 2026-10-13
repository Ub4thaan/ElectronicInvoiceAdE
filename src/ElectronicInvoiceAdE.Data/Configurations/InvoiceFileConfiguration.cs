using ElectronicInvoiceAdE.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicInvoiceAdE.Data.Configurations;

public class InvoiceFileConfiguration : IEntityTypeConfiguration<InvoiceFileEntity>
{
	public void Configure(EntityTypeBuilder<InvoiceFileEntity> builder)
	{
		ArgumentNullException.ThrowIfNull(builder);
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
