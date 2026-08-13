using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Tenant;

namespace Shifter.Core.Entities.Timesheets;

public class BreakEvent : BaseEntity, IMultiTenant
{
    public Guid CompanyId { get; set; }

    public Guid UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; } = null!;

    public string Name { get; set; } = string.Empty; // Name of the break event (e.g., "Lunch Break", "Coffee Break")

    public DateTime StartTime { get; set; } 
    public DateTime EndTime { get; set; }

    public bool IsPaid { get; set; } 
    public string Notes { get; set; } = string.Empty; 
}
