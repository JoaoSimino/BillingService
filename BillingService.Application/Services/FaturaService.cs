using BillingService.Domain.Entities;
using BillingService.Infrastructure.Data;

namespace BillingService.Application.Services;

public class FaturaService : CrudService<Fatura> , IFaturaService
{
    public FaturaService(BillingServiceContext context) : base(context)
    {
    }
}
