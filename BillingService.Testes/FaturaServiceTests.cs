using BillingService.Application.Services;
using BillingService.Domain.Entities;
using BillingService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Testes;

public class FaturaServiceTests
{
    private BillingServiceContext GetInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<BillingServiceContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new BillingServiceContext(options);
    }


    [Fact]
    public async Task GetFaturaByClientIdAsync_ReturnsFatura_WhenExists()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var proposta = new PropostaAprovadaEvent
        {
            Id = Guid.NewGuid(),
            ClienteId = clientId.ToString(),
            PropostaId = "123",
            ValorAprovado = 1000,
            DataRecebimento = DateTime.Now,
        };

        var fatura = new Fatura
        {
            Id = Guid.NewGuid(),
            PropostaAprovadaEventId = proposta.Id,
            NumeroParcelas = 3,
            DataCriacao = DateTime.Now,
            Status = StatusFatura.Pendente,
            PropostaAprovadaEvent = proposta
        };

        using var context = GetInMemoryContext("FaturaTest1");
        context.Add(proposta);
        context.Add(fatura);
        context.SaveChanges();

        var service = new FaturaService(context);

        // Act
        var result = await service.GetFaturaByClientIdAsync(clientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fatura.Id, result.Id);
    }


    [Fact]
    public async Task GetFaturaByClientIdAsync_Throws_WhenNotFound()
    {
        // Arrange
        using var context = GetInMemoryContext("FaturaTest2");
        var service = new FaturaService(context);
        var invalidClientId = Guid.NewGuid();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<Exception>(() => service.GetFaturaByClientIdAsync(invalidClientId));
        Assert.Equal("Fatura nao encontrada para o Cliente em questao", ex.Message);
    }
}