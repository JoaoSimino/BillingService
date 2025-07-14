namespace BillingService.Domain.DTOs;

public record OpcaoPagamentoDto(int QuantidadeParcelas, decimal ValorParcela, decimal ValorTotalComJuros , bool ComJuros);
