using Microsoft.EntityFrameworkCore;
using ElectronicInvoiceAdE.Data.Entities;
using ElectronicInvoiceAdE.Data.Configurations;

namespace ElectronicInvoiceAdE.Data;

/// <summary>
/// A generic DbContext that allows consumers to supply their own derived entity types.
/// </summary>
public class ElectronicInvoiceDbContext<TFile, TOrdinary, TSimplified, TBody, TLineItem> : DbContext
    where TFile : InvoiceFileEntity
    where TOrdinary : OrdinaryInvoiceEntity
    where TSimplified : SimplifiedInvoiceEntity
    where TBody : InvoiceBodyEntity
    where TLineItem : InvoiceLineItemEntity
{
    public ElectronicInvoiceDbContext(DbContextOptions options) : base(options) { }

    public DbSet<TFile> InvoiceFiles { get; set; } = null!;
    public DbSet<TOrdinary> OrdinaryInvoices { get; set; } = null!;
    public DbSet<TSimplified> SimplifiedInvoices { get; set; } = null!;
    public DbSet<TBody> InvoiceBodies { get; set; } = null!;
    public DbSet<TLineItem> InvoiceLineItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // We apply the default configurations, but since we have a generic DbContext,
        // we can't just use ApplyConfigurationsFromAssembly easily if the user extends the types,
        // unless they also inherit the configurations.
        // For standard setup, we can explicitly configure the relationships here or rely on the base config classes.
        // We will call the base configuration classes for the generic types by casting the builder.
        
        new InvoiceFileConfiguration().Configure(modelBuilder.Entity<InvoiceFileEntity>());
        new OrdinaryInvoiceConfiguration().Configure(modelBuilder.Entity<OrdinaryInvoiceEntity>());
        new SimplifiedInvoiceConfiguration().Configure(modelBuilder.Entity<SimplifiedInvoiceEntity>());
        new InvoiceBodyConfiguration().Configure(modelBuilder.Entity<InvoiceBodyEntity>());
        new InvoiceLineItemConfiguration().Configure(modelBuilder.Entity<InvoiceLineItemEntity>());
    }
}

/// <summary>
/// A convenience DbContext for consumers who want to use the default entities.
/// </summary>
public class ElectronicInvoiceDbContext : ElectronicInvoiceDbContext<
    InvoiceFileEntity, 
    OrdinaryInvoiceEntity, 
    SimplifiedInvoiceEntity, 
    InvoiceBodyEntity, 
    InvoiceLineItemEntity>
{
    public ElectronicInvoiceDbContext(DbContextOptions<ElectronicInvoiceDbContext> options) : base(options) { }
}
