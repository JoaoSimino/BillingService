using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BillingService.Infrastructure.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<BillingServiceContext>
{
    public BillingServiceContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "BillingService.Api"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<BillingServiceContext>();

        optionsBuilder.UseSqlServer(connectionString);

        return new BillingServiceContext(optionsBuilder.Options);
    }
}
