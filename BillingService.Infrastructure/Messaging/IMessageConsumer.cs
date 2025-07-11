namespace BillingService.Infrastructure.Messaging;

public interface IMessageConsumer
{
    Task ReceiveAsync(CancellationToken cancellationToken);
}
