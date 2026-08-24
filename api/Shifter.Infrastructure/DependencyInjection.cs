using Audit.Core;
using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Shifter.Application.Interfaces.Tenant;
using Shifter.Core.Entities.Audit;
using Shifter.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;

namespace Shifter.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        ConfigureAuditLogging();

        return services;
    }

    private static void ConfigureAuditLogging()
    {
        // Tell Audit.NET to ignore the AuditLog entity
        Audit.EntityFramework.Configuration.Setup()
            .ForAnyContext()
            .UseOptOut()
            .Ignore<AuditLog>();

        // Configure the Data Provider
        Audit.Core.Configuration.Setup()
           .UseEntityFramework(ef => ef
               .AuditTypeMapper(t => typeof(AuditLog)) // Map all audit events to the AuditLog entity
               .AuditEntityAction<AuditLog>((ev, entry, entity) =>
               {
                   var dbContext = ev.GetEntityFrameworkEvent().GetDbContext();
                   var tenantService = dbContext.GetService<ITenantService>();

                   entity.CompanyId = tenantService.GetCompanyId();
                   entity.UserProfileId = tenantService.GetCurrentUserId();

                   entity.EntityName = entry.EntityType.Name;
                   entity.EntityId = entry.PrimaryKey.FirstOrDefault().Value?.ToString() ?? string.Empty;
                   entity.Action = entry.Action;
                   entity.CreatedAt = DateTime.UtcNow;

                   entity.Changes = entry.Changes != null
                       ? JsonSerializer.Serialize(entry.Changes)
                       : string.Empty;
               })
               .IgnoreMatchedProperties(true));
    }
}
