using BillingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Infrastructure.Data;

public class BillingServiceContext : DbContext
{
    public BillingServiceContext(DbContextOptions<BillingServiceContext> options) : base(options)
    {
        
    }

    public DbSet<PropostaAprovadaEvent> PropostasAprovadaEvento { get; set; }
    public DbSet<Fatura> Faturas { get; set; }
    public DbSet<Parcela> Parcelas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropostaAprovadaEvent>()
            .HasOne(pae => pae.Fatura)
            .WithOne(f => f.PropostaAprovadaEvent)
            .HasForeignKey<Fatura>(f => f.PropostaAprovadaEventId);

        modelBuilder.Entity<PropostaAprovadaEvent>()
            .OwnsOne(pae => pae.OpcaoPagamentoSelecionada);

        modelBuilder.Entity<Fatura>()
            .HasMany(f => f.Parcelas)
            .WithOne(p => p.Fatura)
            .HasForeignKey(p => p.FaturaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PropostaAprovadaEvent>()
            .Property(pae => pae.StatusProcessamento)
            .HasConversion<string>();

        modelBuilder.Entity<Fatura>()
            .Property(f => f.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Parcela>()
            .Property(p => p.Status)
            .HasConversion<string>();
        modelBuilder.Entity<Parcela>()
            .Property(p => p.DataVencimento)
            .HasColumnType("date");

        base.OnModelCreating(modelBuilder);
    }
}
