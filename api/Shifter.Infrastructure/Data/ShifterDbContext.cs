using Microsoft.EntityFrameworkCore;
using Shifter.Application.Interfaces.Tenant;
using Shifter.Core.Entities.HR;
using Shifter.Core.Entities.Identity;
using Shifter.Core.Entities.Tenant;
using Shifter.Core.Entities.Timesheets;

namespace Shifter.Infrastructure.Data;

public class ShifterDbContext : DbContext
{
    private readonly Guid _currentCompanyId;

    public ShifterDbContext(DbContextOptions<ShifterDbContext> options, ITenantService tenantService)
        : base(options)
    {
        _currentCompanyId = tenantService.GetCompanyId();
    }

    public ShifterDbContext(DbContextOptions<ShifterDbContext> options)
        : base(options)
    {
        _currentCompanyId = Guid.Empty;
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<WorkEvent> WorkEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply global query filter for multi-tenancy to use the current company ID
        modelBuilder.Entity<User>().HasQueryFilter(u => u.CompanyId == _currentCompanyId);
        modelBuilder.Entity<EmployeeProfile>().HasQueryFilter(e => e.CompanyId == _currentCompanyId);
        modelBuilder.Entity<WorkEvent>().HasQueryFilter(w => w.CompanyId == _currentCompanyId);
    }
}

