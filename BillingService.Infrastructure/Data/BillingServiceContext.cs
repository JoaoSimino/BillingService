using BillingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BillingService.Infrastructure.Data;

public class BillingServiceContext : DbContext
{
    public BillingServiceContext(DbContextOptions<BillingServiceContext> options) : base(options)
    {
        
    }

    public DbSet<PropostaAprovadaEvent> PropostasAprovadaEvento { get; set; }
}
