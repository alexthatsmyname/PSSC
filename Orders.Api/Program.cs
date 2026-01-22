using Microsoft.EntityFrameworkCore;
using Orders.Api.Domain.Operations;
using Orders.Api.Domain.Repositories;
using Orders.Api.Domain.Workflows;
using Orders.Api.Infrastructure;
using Orders.Api.Infrastructure.Repositories;
using Orders.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseSqlServer(connectionString));

// Repository
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Operations
builder.Services.AddScoped<TransformPlaceOrderOperation>();
builder.Services.AddScoped<ValidatePlaceOrderOperation>();
builder.Services.AddScoped<CreateOrderOperation>();
builder.Services.AddScoped<SetOrderStatusOperation>();
builder.Services.AddScoped<PersistOrderOperation>();

// Workflow
builder.Services.AddScoped<PlaceOrderWorkflow>();

// HTTP Client for Billing API
builder.Services.AddHttpClient<BillingApiClient>(client =>
{
    var billingApiUrl = builder.Configuration["BillingApi:BaseUrl"] ?? "http://localhost:5002";
    client.BaseAddress = new Uri(billingApiUrl);
});

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
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
