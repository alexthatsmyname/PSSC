using Microsoft.EntityFrameworkCore;
using Orders.Api.Domain.Models;

namespace Orders.Api.Infrastructure;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

            builder.OwnsMany(o => o.Items, itemBuilder =>
            {
                itemBuilder.WithOwner().HasForeignKey("OrderId");
                itemBuilder.HasKey("Id");

                itemBuilder.Property<int>("Id").ValueGeneratedOnAdd();

                itemBuilder.Property(i => i.ProductId)
                    .IsRequired()
                    .HasMaxLength(128);

                itemBuilder.Property(i => i.ProductName)
                    .IsRequired()
                    .HasMaxLength(256);

                itemBuilder.Property(i => i.Quantity)
                    .IsRequired();

                itemBuilder.Property(i => i.UnitPrice)
                    .HasPrecision(18, 2)
                    .IsRequired();
            });
        });
    }
}
