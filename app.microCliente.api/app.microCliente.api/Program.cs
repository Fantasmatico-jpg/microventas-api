using app.microCliente.common.EventMQ;
using app.microCliente.dataAccess.context;
using app.microCliente.dataAccess.repositories;
using app.microCliente.services.EventMQ;
using app.microCliente.services.Implementations;
using app.microCliente.services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cadena de conexión a SQL Server
var conSqlServer =
    builder.Configuration.GetConnectionString("BDDSqlServer")!;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(conSqlServer);

    options.LogTo(
        Console.WriteLine,
        LogLevel.Information
    )
    .EnableSensitiveDataLogging();
});

// Configuración de RabbitMQ
builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("rabbitmq")
);

// Repositories y Services
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<IDireccionClienteRepository, DireccionClienteRepository>();
builder.Services.AddScoped<IDireccionClienteService, DireccionClienteService>();

// Historial de compras
builder.Services.AddScoped<IHistorialCompraRepository, HistorialCompraRepository>();
builder.Services.AddScoped<IHistorialCompraService, HistorialCompraService>();

// Servicios RabbitMQ
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();
builder.Services.AddSingleton<IRabbitMQConsumerService, RabbitMQConsumerService>();

var app = builder.Build();

// Iniciar consumidor de RabbitMQ
var consumerService =
    app.Services.GetRequiredService<IRabbitMQConsumerService>();

await consumerService.StartConsumerAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsEnvironment("Docker"))
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();