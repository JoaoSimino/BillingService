using BillingService.Domain.Entities;

namespace BillingService.Application.Services;

public interface IParcelaService : ICrudService<Parcela>
{
    public Task<IEnumerable<Parcela>> GetParcelaByFaturaIdAsync(Guid id);

    public Task<IEnumerable<Parcela>> GetParcelaByClienteIdAsync(Guid id);
    public Task RealizarPagamentoAsync(Guid id);
}
