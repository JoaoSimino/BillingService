using BillingService.Domain.Entities;
using BillingService.Infrastructure.Data;

namespace BillingService.Application.Services;

public class PropostaAprovadaEventService : CrudService<PropostaAprovadaEvent>, IPropostaAprovadaEventService
{
    public PropostaAprovadaEventService(BillingServiceContext context) : base(context)
    {     
    }
}


