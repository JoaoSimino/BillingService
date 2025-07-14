using BillingService.Domain.Entities;

namespace BillingService.Application.Services;

public interface IParcelaService : ICrudService<Parcela>
{
    public Task<IEnumerable<Parcela>> GetParcelaByFaturaId(Guid id);
}
