
using BillingService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace BillingService.Infrastructure.Messaging;

public class RabbitMQConsumer : IMessageConsumer
{
    private readonly ILogger<RabbitMQConsumer> _logger;
    private readonly ConnectionFactory _factory;



    public RabbitMQConsumer(ILogger<RabbitMQConsumer> logger, IConfiguration configuration)
    {
        _logger = logger;
        _factory = new ConnectionFactory
        { 
            HostName = configuration["RabbitMQ:HostName"] ??"localhost"
        };
    }


    public async Task ReceiveAsync(Func<PropostaAprovadaEvent, Task> handleEvent, CancellationToken cancellationToken)
    {
        using var connection = await _factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "propostas-aprovadas",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var mensagem = Encoding.UTF8.GetString(body);

            try
            {
                var evento = JsonSerializer.Deserialize<PropostaAprovadaEvent>(mensagem);
                _logger.LogInformation("Evento recebido: {PropostaId}", evento?.PropostaId);
                Console.WriteLine($"Evento recebido: {evento?.PropostaId}" );

                if (evento is not null)//chamando evento delegado que deve efetuar logica e ter acesso aos servicos da API!
                    await handleEvent(evento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem.");
                Console.WriteLine("Erro ao processar mensagem.");
            }
        };
        
        await channel.BasicConsumeAsync(queue: "propostas-aprovadas", autoAck: true, consumer: consumer);

        _logger.LogInformation("RabbitMQConsumer ativo e escutando fila.");
        Console.WriteLine("RabbitMQConsumer ativo e escutando fila.");//usando temporaiamente o cw

        // Mantém o consumidor vivo enquanto não for cancelado
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000, cancellationToken);
        }

        await channel.CloseAsync();
        await connection.CloseAsync();

    }

}
