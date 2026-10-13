using ElectronicInvoiceAdE.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElectronicInvoiceAdE.Data.Configurations;

public class SimplifiedInvoiceConfiguration : IEntityTypeConfiguration<SimplifiedInvoiceEntity>
{
	public void Configure(EntityTypeBuilder<SimplifiedInvoiceEntity> builder)
	{
		ArgumentNullException.ThrowIfNull(builder);
		builder.HasKey(x => x.Id);
	}
}
