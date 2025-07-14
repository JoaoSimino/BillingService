using BillingService.Domain.DTOs;
using BillingService.Domain.Entities;

namespace BillingService.Application.Services;

public interface IPropostaAprovadaEventService : ICrudService<PropostaAprovadaEvent>
{
    public Task<List<OpcaoPagamento>> CalcularOpcoesPagamentoAsync(Guid id);

    public Task InformarOpcaoSelecionada(Guid id, OpcaoPagamentoDto opcaodto);
}