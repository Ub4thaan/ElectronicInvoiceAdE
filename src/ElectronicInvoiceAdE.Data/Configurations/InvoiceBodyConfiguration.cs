using ElectronicInvoiceAdE.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicInvoiceAdE.Data.Configurations;

public class InvoiceBodyConfiguration : IEntityTypeConfiguration<InvoiceBodyEntity>
{
	public void Configure(EntityTypeBuilder<InvoiceBodyEntity> builder)
	{
		ArgumentNullException.ThrowIfNull(builder);
		builder.HasKey(x => x.Id);

		builder.HasMany(x => x.LineItems)
			   .WithOne(x => x.InvoiceBody)
			   .HasForeignKey(x => x.InvoiceBodyId)
			   .OnDelete(DeleteBehavior.Cascade);
	}
}
