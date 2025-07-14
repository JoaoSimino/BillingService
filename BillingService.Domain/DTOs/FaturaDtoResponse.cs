using BillingService.Domain.Entities;

namespace BillingService.Domain.DTOs;

public record FaturaDtoResponse
{
    public Guid Id { get; set; }
    public int NumeroParcelas { get; set; }
    public StatusFatura Status { get; set; }
}