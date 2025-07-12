namespace BillingService.Domain.Entities;

public class PropostaAprovadaEvent
{
    public Guid Id { get; set; }
    public string PropostaId { get; set; } = default!;
    public string ClienteId { get; set; } = default!;
    public decimal ValorAprovado { get; set; }

    public DateTime DataRecebimento { get; set; }

}
