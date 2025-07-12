using BillingService.Application.Services;
using BillingService.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMessageConsumer, RabbitMQConsumer>();
builder.Services.AddScoped<IPropostaAprovadaEventService, PropostaAprovadaEventService>();


var app = builder.Build();

var consumer = app.Services.GetRequiredService<IMessageConsumer>();
var propostaService = app.Services.GetRequiredService<IPropostaAprovadaEventService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//_ = consumer.ReceiveAsync(CancellationToken.None);// inicia a escuta em background
_ = consumer.ReceiveAsync(async evento => {
    evento.DataRecebimento = DateTime.UtcNow;
    await propostaService.AddAsync(evento);
}, CancellationToken.None);


app.Run();


