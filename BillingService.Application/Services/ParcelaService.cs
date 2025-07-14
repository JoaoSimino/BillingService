using BillingService.Domain.Entities;
using BillingService.Infrastructure.Data;

namespace BillingService.Application.Services;

public class ParcelaService : CrudService<Parcela> ,IParcelaService
{
    public ParcelaService(BillingServiceContext context) : base(context)
    {    
    }
}
