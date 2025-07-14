using BillingService.Domain.DTOs;
using BillingService.Domain.Entities;

namespace BillingService.Application.Services;

public interface IFaturaService : ICrudService<Fatura>
{
    public Task<FaturaDtoResponse> GetFaturaByClientIdAsync(Guid id);
}
