namespace BillingService.Domain.Entities;

public class Parcela
{
    public Guid Id { get; set; }

    public Guid FaturaId { get; set; }
    public Fatura Fatura { get; set; } = default!;
    public int Numero { get; set; }

    public decimal Valor { get; set; }
    public DateTime DataVencimento { get; set; }

    public StatusParcela Status { get; set; }
    public DateTime? DataPagamento { get; set; }
}

