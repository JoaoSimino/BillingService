namespace BillingService.Domain.Entities;

public class OpcaoPagamento
{
    public int QuantidadeParcelas { get; set; }
    public decimal ValorParcela { get; set; }
    public decimal ValorTotalComJuros { get; set; }
    public bool ComJuros { get; set; }
}
