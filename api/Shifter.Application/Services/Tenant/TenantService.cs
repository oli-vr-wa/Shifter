using Shifter.Application.Interfaces.Tenant;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Shifter.Application.Services.Tenant;

public class TenantService(IHttpContextAccessor httpContextAccessor) : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    /// <inheritdoc />
    public Guid GetCompanyId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var companyIdClaim = user?.FindFirst("CompanyId")?.Value;

        if (Guid.TryParse(companyIdClaim, out var companyId))        
            return companyId;

        return Guid.Empty;
    }

    /// <inheritdoc />
    public Guid GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdClaim, out var userId))
            return userId;
        return Guid.Empty;
    }
}
