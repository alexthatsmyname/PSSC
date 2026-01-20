using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectPSSC.Domain.Repositories;
using ProiectPSSC.Infrastructure.Persistence;
using ProiectPSSC.Infrastructure.Repositories;

namespace ProiectPSSC.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<PsscDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IShipmentRepository, ShipmentRepository>();
            services.AddScoped<ProjectPSSC.Domain.Operations.ValidateShipmentCreationOperation>();
            services.AddScoped<ProjectPSSC.Domain.Operations.TransformOrderToShipmentOperation>();
            services.AddScoped<ProjectPSSC.Domain.Operations.PersistShipmentOperation>();
            services.AddScoped<ProjectPSSC.Domain.Operations.SetOrderShippedOperation>();
            services.AddScoped<ProjectPSSC.Domain.Workflows.CreateShipmentWorkflow>();

        return services;
    }
}
