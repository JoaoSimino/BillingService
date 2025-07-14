using BillingService.Domain.DTOs;
using BillingService.Domain.Entities;
using BillingService.Infrastructure.Data;

namespace BillingService.Application.Services;

public class PropostaAprovadaEventService : CrudService<PropostaAprovadaEvent>, IPropostaAprovadaEventService
{
    private readonly IFaturaService _faturaService;
    private readonly IParcelaService _parcelaService;

    public PropostaAprovadaEventService(BillingServiceContext context, IFaturaService faturaService, IParcelaService parcelaService) : base(context)
    {
        _faturaService = faturaService;
        _parcelaService = parcelaService;
    }

    public async Task<List<OpcaoPagamento>> CalcularOpcoesPagamentoAsync(Guid id)
    {
        
        var propostaAprovada = await _context.Set<PropostaAprovadaEvent>().FindAsync(id);
        if (propostaAprovada is null)
            throw new Exception("Proposta aprovada não encontrada.");

        decimal valor = propostaAprovada.ValorAprovado;
        var opcoes = new List<OpcaoPagamento>();

        int limiteSemJuros = valor <= 50000 ? 2 : 6;//verificando ate quantas parcelas eh possivel sem juros com base no valor!
        int maxParcelas = valor <= 50000 ? 10 : 24;
        decimal jurosMensal = 0.01m;

        for (int parcelas = 1; parcelas <= maxParcelas; parcelas++) 
        {
            bool comJuros = parcelas > limiteSemJuros;
            decimal valorTotal;
            decimal valorParcela;

            if (comJuros)
            {
                // juros simples: ValorTotal = valor * (1 + juros * meses)
                valorTotal = valor * (1 + jurosMensal * parcelas);
            }
            else
            {
                valorTotal = valor;
            }

            valorParcela = Math.Round(valorTotal / parcelas, 2);

            opcoes.Add(new OpcaoPagamento
            {
                QuantidadeParcelas = parcelas,
                ValorParcela = valorParcela,
                ValorTotalComJuros = Math.Round(valorTotal, 2),
                ComJuros = comJuros
            });
        }

        return opcoes;


    }

    public async Task InformarOpcaoSelecionada(Guid id, OpcaoPagamentoDto opcaoDto)
    {
        var opcao = new OpcaoPagamento().DtoToEntity(opcaoDto);
        var propostaAprovada = await _context.Set<PropostaAprovadaEvent>().FindAsync(id);
        if (propostaAprovada is null)
            throw new Exception("Proposta aprovada não encontrada.");
        propostaAprovada.OpcaoPagamentoSelecionada = opcao;

        //logica para geracao de fatura, ja com as informacoes necessarias!

        var idFatura = Guid.NewGuid();
        var numeroDeParcelas = propostaAprovada.OpcaoPagamentoSelecionada.QuantidadeParcelas;

        var ListaDeParcelas = new List<Parcela>();
        
        for (int i =0; i < numeroDeParcelas; i++) 
        {
            var parcela = new Parcela
            {
                Id = Guid.NewGuid(),
                FaturaId = idFatura,
                Numero = i + 1,

                Valor = propostaAprovada.OpcaoPagamentoSelecionada.ValorParcela,

                Status = StatusParcela.Pendente,
                DataVencimento = DateTime.Now.AddMonths(i + 1),
            };
            ListaDeParcelas.Add(parcela);
        }

        var fatura = new Fatura{
            Id = idFatura,
            PropostaAprovadaEventId = propostaAprovada.Id,
            NumeroParcelas = numeroDeParcelas,
            DataCriacao = DateTime.Now,
            Status = StatusFatura.Pendente,
            Parcelas = ListaDeParcelas
        };

        await _context.AddAsync(fatura);
        propostaAprovada.StatusProcessamento = StatusProcessamento.FaturaGerada;
        await _context.SaveChangesAsync();
        
    }
}


