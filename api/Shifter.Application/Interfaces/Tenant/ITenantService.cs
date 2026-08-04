
namespace Shifter.Application.Interfaces.Tenant;

public interface ITenantService
{
    /// <summary>
    /// Gets the company ID of the currently authenticated user.
    /// </summary>
    /// <returns>GUID representing the company ID</returns>
    Guid GetCompanyId();

    /// <summary>
    /// Gets the user ID of the currently authenticated user.
    /// </summary>
    /// <returns>GUID representing the user ID</returns>
    Guid GetCurrentUserId();
}

