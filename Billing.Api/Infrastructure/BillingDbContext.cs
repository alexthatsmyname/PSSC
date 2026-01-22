using Microsoft.EntityFrameworkCore;
using Billing.Api.Domain.Models;

namespace Billing.Api.Infrastructure;

public class BillingDbContext : DbContext
{
    public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options)
    {
    }

    public DbSet<Invoice> Invoices => Set<Invoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Invoice>(builder =>
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.OrderId)
                .IsRequired();

            builder.Property(i => i.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(i => i.IssuedAt)
                .IsRequired();

            builder.Property(i => i.Status)
                .HasConversion<string>()
                .IsRequired();
        });
    }
}
