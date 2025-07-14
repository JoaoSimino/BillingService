using BillingService.Application.Services;
using BillingService.Domain.Entities;

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



    }
}
