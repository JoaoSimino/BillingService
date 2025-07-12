using BillingService.Domain.Entities;

namespace BillingService.Infrastructure.Messaging;

public interface IMessageConsumer
{
    //Task ReceiveAsync(CancellationToken cancellationToken);
    Task ReceiveAsync(Func<PropostaAprovadaEvent, Task> handleEvent, CancellationToken cancellationToken);

}
