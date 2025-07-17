using BillingService.API.Endpoints;
using BillingService.Application.Services;
using BillingService.Infrastructure.Data;
using BillingService.Infrastructure.Messaging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<BillingServiceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IMessageConsumer, RabbitMQConsumer>();
builder.Services.AddScoped<IPropostaAprovadaEventService, PropostaAprovadaEventService>();
builder.Services.AddScoped<IFaturaService, FaturaService>();
builder.Services.AddScoped<IParcelaService, ParcelaService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermissiveCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


var app = builder.Build();

using (var scopedb = app.Services.CreateScope())
{
    var config = scopedb.ServiceProvider.GetRequiredService<IConfiguration>();
    var fullConnectionString = config.GetConnectionString("DefaultConnection");

    var builderWithoutDb = new SqlConnectionStringBuilder(fullConnectionString)
    {
        InitialCatalog = "master"
    };

    var databaseName = new SqlConnectionStringBuilder(fullConnectionString).InitialCatalog;

    using var connection = new SqlConnection(builderWithoutDb.ConnectionString);
    connection.Open();

    var command = connection.CreateCommand();
    command.CommandText = $@"
        IF DB_ID(N'{databaseName}') IS NULL
        BEGIN
            CREATE DATABASE [{databaseName}];
        END";
    command.ExecuteNonQuery();

    // Aplica migrations
    var dbContext = scopedb.ServiceProvider.GetRequiredService<BillingServiceContext>();
    dbContext.Database.Migrate();
}


var consumer = app.Services.GetRequiredService<IMessageConsumer>();

//migrar futuramente para solucao com BackgroundService , e e injetar os servicos corretamente no construtor.
//como alternativa mais escalavel
using var scope = app.Services.CreateScope();
var propostaService = scope.ServiceProvider.GetRequiredService<IPropostaAprovadaEventService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermissiveCors");

app.UseHttpsRedirection();

//_ = consumer.ReceiveAsync(CancellationToken.None);// inicia a escuta em background
_ = consumer.ReceiveAsync(async evento => {
    evento.DataRecebimento = DateTime.UtcNow;
    await propostaService.AddAsync(evento);
}, CancellationToken.None);


app.MapPropostaAprovadaEndpoints();
app.MapParcelaEndpoints();
app.MapFaturaEndpoints();
app.Run();


