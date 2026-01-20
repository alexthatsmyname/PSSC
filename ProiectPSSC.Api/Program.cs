using ProjectPSSC.Domain.Operations;
using ProjectPSSC.Domain.Workflows;
using ProiectPSSC.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Domain Operations
builder.Services.AddScoped<TransformPlaceOrderOperation>();
builder.Services.AddScoped<ValidatePlaceOrderOperation>();
builder.Services.AddScoped<CreateOrderOperation>();
builder.Services.AddScoped<SetOrderStatusOperation>();
builder.Services.AddScoped<PersistOrderOperation>();

// Register Domain Workflow
builder.Services.AddScoped<PlaceOrderWorkflow>();

// Register Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();