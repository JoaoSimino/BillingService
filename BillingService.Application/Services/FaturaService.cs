using BillingService.Domain.DTOs;
using BillingService.Domain.Entities;
using BillingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Application.Services;

public class FaturaService : CrudService<Fatura> , IFaturaService
{
    public FaturaService(BillingServiceContext context) : base(context)
    {
    }

    public async Task<FaturaDtoResponse> GetFaturaByClientIdAsync(Guid clientId)
    {
        var fatura = await _context.Set<Fatura>()
            .Include(f => f.PropostaAprovadaEvent)
            .Where(f => f.PropostaAprovadaEvent.ClienteId == clientId.ToString()).FirstOrDefaultAsync();
        if (fatura is not null) 
        {
            
            return fatura.EntityToDto(new FaturaDtoResponse());

        }
        throw new Exception("Fatura nao encontrada para o Cliente em questao");
    }
}
