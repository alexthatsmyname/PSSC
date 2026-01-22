using Microsoft.EntityFrameworkCore;
using Shipping.Api.Domain.Operations;
using Shipping.Api.Domain.Repositories;
using Shipping.Api.Domain.Workflows;
using Shipping.Api.Infrastructure;
using Shipping.Api.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ShippingDbContext>(options =>
    options.UseSqlServer(connectionString));

// Repository
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();

// Operations
builder.Services.AddScoped<ValidateInvoiceEventOperation>();
builder.Services.AddScoped<TransformEventToShipmentOperation>();
builder.Services.AddScoped<PersistShipmentOperation>();

// Workflow
builder.Services.AddScoped<CreateShipmentWorkflow>();

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ShippingDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
