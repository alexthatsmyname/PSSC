using Microsoft.EntityFrameworkCore;
using Billing.Api.Domain.Operations;
using Billing.Api.Domain.Repositories;
using Billing.Api.Domain.Workflows;
using Billing.Api.Infrastructure;
using Billing.Api.Infrastructure.Repositories;
using Billing.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<BillingDbContext>(options =>
    options.UseSqlServer(connectionString));

// Repository
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

// Operations
builder.Services.AddScoped<ValidateOrderEventOperation>();
builder.Services.AddScoped<TransformEventToInvoiceOperation>();
builder.Services.AddScoped<PersistInvoiceOperation>();

// Workflow
builder.Services.AddScoped<GenerateInvoiceWorkflow>();

// HTTP Client for Shipping API
builder.Services.AddHttpClient<ShippingApiClient>(client =>
{
    var shippingApiUrl = builder.Configuration["ShippingApi:BaseUrl"] ?? "http://localhost:5003";
    client.BaseAddress = new Uri(shippingApiUrl);
});

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BillingDbContext>();
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
