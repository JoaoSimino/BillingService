using BillingService.Domain.DTOs;

namespace BillingService.Domain.Entities;

public static class OpcaoPagamentoExtensions
{
    public static OpcaoPagamento DtoToEntity(this OpcaoPagamento opcao, OpcaoPagamentoDto opcaoDto) 
    {
        opcao.QuantidadeParcelas = opcaoDto.QuantidadeParcelas;
        opcao.ValorParcela = opcaoDto.ValorParcela;
        opcao.ValorTotalComJuros = opcaoDto.ValorTotalComJuros;
        opcao.ComJuros = opcaoDto.ComJuros;

        return opcao;
    }
}
