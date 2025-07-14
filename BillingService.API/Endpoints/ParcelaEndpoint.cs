using BillingService.Application.Services;
using BillingService.Domain.Entities;

namespace BillingService.API.Endpoints;

public static class ParcelaEndpoint
{
    public static void MapParcelaEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Parcela").WithTags(nameof(Parcela));

        group.MapGet("/{id}", async (IParcelaService service, Guid id) =>
        {
            var listaDePropostasAprovadas = await service.GetAllAsync();

            return listaDePropostasAprovadas.Select(p => new {
            });
        })
        .WithName("GetAllInstallmentByInvoice")
        .WithOpenApi();

    }

}
