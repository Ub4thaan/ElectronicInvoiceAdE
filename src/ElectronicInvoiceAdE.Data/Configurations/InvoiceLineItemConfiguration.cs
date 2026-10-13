using ElectronicInvoiceAdE.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicInvoiceAdE.Data.Configurations;

public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItemEntity>
{
	public void Configure(EntityTypeBuilder<InvoiceLineItemEntity> builder)
	{
		ArgumentNullException.ThrowIfNull(builder);
		builder.HasKey(x => x.Id);
	}
}
