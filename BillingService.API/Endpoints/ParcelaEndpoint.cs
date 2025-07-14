using BillingService.Application.Services;
using BillingService.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BillingService.API.Endpoints;

public static class ParcelaEndpoint
{
    public static void MapParcelaEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Parcelas").WithTags(nameof(Parcela));

        group.MapGet("/Fatura/{id}", async (IParcelaService service, Guid id) =>
        {
            var listaDeParcelas = await service.GetParcelaByFaturaIdAsync(id);

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

        group.MapPost("/{id}/Pagamento", async (Guid id,IParcelaService service) =>
        {
            await service.RealizarPagamentoAsync(id);
            return TypedResults.Ok();
           
        })
        .WithName("MakePaymentByInstallmentID")
        .WithOpenApi();

    }

}
