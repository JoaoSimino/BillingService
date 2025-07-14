using BillingService.Domain.Entities;
using BillingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Application.Services;

public class ParcelaService : CrudService<Parcela> ,IParcelaService
{
    public ParcelaService(BillingServiceContext context) : base(context)
    {    
    }

    public async Task<IEnumerable<Parcela>> GetParcelaByFaturaIdAsync(Guid faturaId)
    {
        return await _context.Set<Parcela>()
            .Include(p => p.Fatura)
            .OrderBy(p => p.Numero)
            .Where(p => p.FaturaId == faturaId).ToListAsync();
    }

    public async Task RealizarPagamentoAsync(Guid parcelaId)
    {
        var parcela = await _context.Set<Parcela>()
            .FindAsync(parcelaId);
        if (parcela is not null && parcela.Status == StatusParcela.Pendente) 
        {
            parcela.Status = StatusParcela.Paga;
            parcela.DataPagamento = DateTime.Now;
        }
        await _context.SaveChangesAsync();
    }
}
