using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Shifter.Infrastructure.Data;

/// <summary>
/// Factory class for creating instances of ShifterDbContext at design time used by EF Core tools (Migrations).
/// </summary>
public class ShifterDbContextFactory : IDesignTimeDbContextFactory<ShifterDbContext>
{
    public ShifterDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Shifter.API"))
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ShifterDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        // Since we don't have access to ITenantService at design time, we can pass null or a mock implementation.
        return new ShifterDbContext(optionsBuilder.Options, null!);
    }
}
