using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shifter.Application.Interfaces.Tenant;
using Shifter.Core.Entities.Identity;
using Shifter.Core.Entities.Tenant;
using Shifter.Core.Entities.Timesheets;

namespace Shifter.Infrastructure.Data;

public class ShifterDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    private readonly ITenantService _tenantService;

    public ShifterDbContext(DbContextOptions<ShifterDbContext> options, ITenantService? tenantService)
        : base(options)
    {
        _tenantService = tenantService!;
    }

    public DbSet<UserProfile> EmployeeProfiles { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<WorkEvent> WorkEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b => b.ToTable("Users"));
        modelBuilder.Entity<IdentityRole<Guid>>(b => b.ToTable("Roles"));

        // Apply global query filter for multi-tenancy to use the current company ID>
        modelBuilder.Entity<UserProfile>().HasQueryFilter(e => e.CompanyId == _tenantService!.GetCompanyId());
        modelBuilder.Entity<WorkEvent>().HasQueryFilter(w => w.CompanyId == _tenantService!.GetCompanyId());
        modelBuilder.Entity<User>().HasQueryFilter(u => u.CompanyId == _tenantService!.GetCompanyId());
    }
}

