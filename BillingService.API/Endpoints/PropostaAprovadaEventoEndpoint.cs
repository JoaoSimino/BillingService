using BillingService.Application.Services;
using BillingService.Domain.DTOs;
using BillingService.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BillingService.API.Endpoints;

public static class PropostaAprovadaEventoEndpoint
{
    public static void MapPropostaAprovadaEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/PropostaAprovada").WithTags(nameof(PropostaAprovadaEvent));

        group.MapGet("/", async (IPropostaAprovadaEventService service) =>
        {
            var listaDePropostasAprovadas = await service.GetAllAsync();

            return listaDePropostasAprovadas.Select(p=> new {
                p.Id,
                p.PropostaId,
                p.ClienteId,
                p.ValorAprovado,
                p.DataRecebimento,
                p.StatusProcessamento
            });
        })
        .WithName("GetAllProposals")
        .WithOpenApi();

        group.MapGet("/{id}", async (IPropostaAprovadaEventService service, Guid id) =>
        {
            var propostaAprovada = await service.GetByIdAsync(id);

            return new {
                propostaAprovada.Id,
                propostaAprovada.PropostaId,
                propostaAprovada.ClienteId,
                propostaAprovada.ValorAprovado,
                propostaAprovada.DataRecebimento,
                propostaAprovada.StatusProcessamento
            };
        })
        .WithName("GetAllProposalsById")
        .WithOpenApi();

        group.MapGet("/{id}/opcoes-de-pagamento", async (IPropostaAprovadaEventService service, Guid id) =>
        {
       
            return await service.CalcularOpcoesPagamentoAsync(id);
        })
        .WithName("GetAllPaymentOptionsByProposalId")
        .WithOpenApi();

        group.MapPost("/{id}/opcao-de-pagamento-selecionada", async (
            Guid id,
            IPropostaAprovadaEventService service,
            OpcaoPagamentoDto opcaoDto
            ) =>
        {
            await service.InformarOpcaoSelecionada(id, opcaoDto);
            return TypedResults.Ok();
        })
        .WithName("SetPaymentOptionsByProposalId")
        .WithOpenApi();

    }
}
