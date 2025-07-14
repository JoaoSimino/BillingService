using BillingService.Application.Services;
using BillingService.Domain.Entities;

namespace BillingService.API.Endpoints;

public static class ParcelaEndpoint
{
    public static void MapParcelaEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Parcelas").WithTags(nameof(Parcela));

        group.MapGet("/Fatura/{id}", async (IParcelaService service, Guid id) =>
        {
            var listaDeParcelas = await service.GetParcelaByFaturaId(id);

            return listaDeParcelas.Select(p => new {
                p.Id,
                p.Numero,
                p.DataVencimento,
                p.Valor,
                p.Status
            });
        })
        .WithName("GetAllInstallmentByInvoice")
        .WithOpenApi();

    }

}
