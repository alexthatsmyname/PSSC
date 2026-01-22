using Microsoft.EntityFrameworkCore;
using Shipping.Api.Domain.Models;

namespace Shipping.Api.Infrastructure;

public class ShippingDbContext : DbContext
{
    public ShippingDbContext(DbContextOptions<ShippingDbContext> options) : base(options)
    {
    }

    public DbSet<Shipment> Shipments => Set<Shipment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Shipment>(builder =>
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.OrderId)
                .IsRequired();

            builder.Property(s => s.TrackingNumber)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(s => s.CreatedAt)
                .IsRequired();

            builder.Property(s => s.Status)
                .HasConversion<string>()
                .IsRequired();
        });
    }
}
