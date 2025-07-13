namespace BillingService.Domain.Entities;

public class Fatura
{
    public Guid Id { get; set; }

    public Guid PropostaAprovadaEventId { get; set; }
    public PropostaAprovadaEvent PropostaAprovadaEvent { get; set; } = default!;

    public int NumeroParcelas { get; set; }
    public DateTime DataCriacao { get; set; }

    public StatusFatura Status { get; set; }

    public List<Parcela> Parcelas { get; set; } = new();
}