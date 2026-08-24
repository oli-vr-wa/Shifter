using Audit.Core;
using Audit.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Shifter.Application.Interfaces.Tenant;
using Shifter.Core.Entities.Audit;
using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.HumanResources;
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
    private bool _isSavingAuditLogs = false;

    public ShifterDbContext(DbContextOptions<ShifterDbContext> options, ITenantService? tenantService)
        : base(options)
    {
        _auditContext = new DefaultAuditContext(this);
        _tenantService = tenantService!;           
    }

    [AuditIgnore]
    public DbSet<AuditLog> AuditLogs { get; set; } 
    public DbSet<UserProfile> EmployeeProfiles { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<WorkEvent> WorkEvents { get; set; }
    public DbSet<WorkEventHistory> WorkEventHistories { get; set; }
    public DbSet<Address> Addresses { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(b => b.ToTable("Users"));
        modelBuilder.Entity<IdentityRole<Guid>>(b => b.ToTable("Roles"));
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShifterDbContext).Assembly);

        // Apply global query filter for multi-tenancy to use the current company ID>
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var primaryKey = entityType.FindPrimaryKey();

            if (primaryKey != null && primaryKey.Properties.Count == 1)
            {
                var pkProperty = primaryKey.Properties[0];
                if (pkProperty.ClrType == typeof(Guid))
                {
                    pkProperty.ValueGenerated = ValueGenerated.Never;
                }
            }

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
            if (entry.State == EntityState.Added && companyId != Guid.Empty)
            {
                entry.Entity.CompanyId = companyId;
            }
        }
    }

    private void ApplyBaseEntityProperties()
    {
        var currentTime = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = currentTime;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastUpdatedAt = currentTime;
            }
        }
    }

    // Override all save methods to include audit logging
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await ExecuteSaveChangesWithAuditAsync(() => base.SaveChangesAsync(cancellationToken), cancellationToken);    

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) =>
        await ExecuteSaveChangesWithAuditAsync(() => base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken), cancellationToken);

    public override int SaveChanges() => ExecuteSaveChangesWithAudit(() => base.SaveChanges());

    public override int SaveChanges(bool acceptAllChangesOnSuccess) =>
        ExecuteSaveChangesWithAudit(() => base.SaveChanges(acceptAllChangesOnSuccess));


    private async Task<int> ExecuteSaveChangesWithAuditAsync(Func<Task<int>> saveChangesFunc, CancellationToken cancellationToken)
    {
        if (_isSavingAuditLogs)
        {
            return await saveChangesFunc();
        }
        try
        {
            _isSavingAuditLogs = true;
            ApplyMultiTenancy();
            ApplyBaseEntityProperties();
            return await _dbContextHelper.SaveChangesAsync(_auditContext, saveChangesFunc, cancellationToken);
        }
        finally
        {
            _isSavingAuditLogs = false;
        }
    }

    private int ExecuteSaveChangesWithAudit(Func<int> saveChangesFunc)
    {
        if (_isSavingAuditLogs)
        {
            return saveChangesFunc();
        }
        try
        {
            _isSavingAuditLogs = true;
            ApplyMultiTenancy();
            ApplyBaseEntityProperties();
            return _dbContextHelper.SaveChanges(_auditContext, saveChangesFunc);
        }
        finally
        {
            _isSavingAuditLogs = false;
        }
    }
}

