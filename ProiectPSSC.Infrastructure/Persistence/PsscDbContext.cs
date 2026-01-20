using Microsoft.EntityFrameworkCore;
using ProjectPSSC.Domain.Models;

namespace ProiectPSSC.Infrastructure.Persistence;

public class PsscDbContext : DbContext
{
    public PsscDbContext(DbContextOptions<PsscDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Order entity
        modelBuilder.Entity<Order>(builder =>
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.CustomerName)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(o => o.CustomerEmail)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(o => o.ShippingAddress)
                .IsRequired()
                .HasMaxLength(512);

            builder.Property(o => o.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            // Configure owned OrderItem collection
            builder.OwnsMany(o => o.Items, builder =>
            {
                builder.WithOwner().HasForeignKey("OrderId");
                builder.HasKey("Id");

                builder.Property(i => i.ProductId)
                    .IsRequired()
                    .HasMaxLength(128);

                builder.Property(i => i.ProductName)
                    .IsRequired()
                    .HasMaxLength(256);

                builder.Property(i => i.Quantity)
                    .IsRequired();

                builder.Property(i => i.UnitPrice)
                    .HasPrecision(18, 2)
                    .IsRequired();
            });
        });
    }
}
