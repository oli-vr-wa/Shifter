using Shifter.Application.Interfaces.Tenant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Application.Services.Tenant;

internal class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCompanyId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var companyIdClaim = user?.FindFirst("companyId")?.Value;

        if (Guid.TryParse(companyIdClaim, out var companyId))        
            return companyId;

        throw new UnauthorizedAccessException("Company context could not be resolved");
    }
}
