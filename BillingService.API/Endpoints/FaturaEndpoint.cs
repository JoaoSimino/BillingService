using BillingService.Application.Services;
using BillingService.Domain.Entities;

namespace BillingService.API.Endpoints;

public static class FaturaEndpoint
{
    public static void MapFaturaEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Fatura").WithTags(nameof(Fatura));

        group.MapGet("/Cliente/{id}", async (IFaturaService service, Guid clientId) =>
        {
            var fatura = await service.GetFaturaByClientId(clientId);

            return fatura;
        })
        .WithName("GetAllInvoicesByClient")
        .WithOpenApi();

    }
}
