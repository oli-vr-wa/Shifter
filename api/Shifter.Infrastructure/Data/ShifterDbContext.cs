using Audit.Core;
using Audit.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shifter.Application.Interfaces.Tenant;
using Shifter.Core.Entities.Audit;
using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Identity;
using Shifter.Core.Entities.Location;
using Shifter.Core.Entities.Tenant;
using Shifter.Core.Entities.Timesheets;
using System.Linq.Expressions;
using System.Text.Json;

namespace Shifter.Infrastructure.Data;

public class ShifterDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    private readonly ITenantService _tenantService;
    // The audit context used for tracking changes in the database.
    private readonly IAuditDbContext _auditContext;
    // _dbContextHelper to assist with saving changes and handling audit logs using Audit.NET.
    private readonly DbContextHelper _dbContextHelper = new DbContextHelper();

    public ShifterDbContext(DbContextOptions<ShifterDbContext> options, ITenantService? tenantService)
        : base(options)
    {
        _auditContext = new DefaultAuditContext(this);
        _tenantService = tenantService!;

        // Configure Audit.NET to use the database for logging
        Audit.Core.Configuration.Setup()
            .UseEntityFramework(ef => ef
                .AuditTypeMapper(t => typeof(AuditLog)) // Map all audit events to the AuditLog entity
                .AuditEntityAction<AuditLog>((ev, entry, entity) =>
                {
                    entity.CompanyId = _tenantService.GetCompanyId();
                    entity.EntityName = entry.EntityType.Name;
                    entity.EntityId = entry.PrimaryKey.FirstOrDefault().Value?.ToString() ?? string.Empty;
                    entity.Action = entry.Action;
                    entity.UserProfileId = _tenantService.GetCurrentUserId();
                    entity.CreatedAt = DateTime.UtcNow;

                    entity.Changes = entry.Changes != null
                        ? JsonSerializer.Serialize(entry.Changes)
                        : string.Empty;
                })
                .IgnoreMatchedProperties(true));

        Audit.EntityFramework.Configuration.Setup()
            .ForContext<ShifterDbContext>()
            .UseOptOut()
            .Ignore<AuditLog>();            
    }

    public DbSet<AuditLog> AuditLogs { get; set; } 
    public DbSet<UserProfile> EmployeeProfiles { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<WorkEvent> WorkEvents { get; set; }
    public DbSet<WorkEventHistory> WorkEventHistories { get; set; }
    public DbSet<Address> Addresses { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b => b.ToTable("Users"));
        modelBuilder.Entity<IdentityRole<Guid>>(b => b.ToTable("Roles"));

        // Apply global query filter for multi-tenancy to use the current company ID>
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IMultiTenant).IsAssignableFrom(entityType.ClrType))
            {
                // Create a parameter for the entity type
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                // Create an expression to access the CompanyId property
                var propertyAccess = Expression.Property(parameter, nameof(IMultiTenant.CompanyId));
                // Capture the current company ID from the tenant service using a lambda expression to ensure it's evaluated with every query
                Expression<Func<Guid>> tenantIdExpression = () => _tenantService.GetCompanyId();
                var tenantIdValue = tenantIdExpression.Body;
                // Create an equality expression comparing the CompanyId property to the current company ID
                var equality = Expression.Equal(propertyAccess, tenantIdValue);
                // Create a lambda expression for the filter
                var filterLambda = Expression.Lambda(equality, parameter);
                // Apply the filter to the entity type
                entityType.SetQueryFilter(filterLambda);
            }
        }
    }

    private void ApplyMultiTenancy()
    {
        var companyId = _tenantService.GetCompanyId();

        foreach (var entry in ChangeTracker.Entries<IMultiTenant>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CompanyId = companyId;
            }
        }
    }

    // Override all save methods to include audit logging
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyMultiTenancy();
        return await _dbContextHelper.SaveChangesAsync(_auditContext,
            () => base.SaveChangesAsync(cancellationToken));
    }

    public override int SaveChanges()
    {
        ApplyMultiTenancy();
        return _dbContextHelper.SaveChanges(_auditContext,
            () => base.SaveChanges());
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyMultiTenancy();
        return await _dbContextHelper.SaveChangesAsync(_auditContext,
            () => base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
    }
}

