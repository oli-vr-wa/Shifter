using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Tenant;
using Shifter.Core.Entities.Timesheets.Enums;

namespace Shifter.Core.Entities.Timesheets;

public class WorkEventHistory : BaseEntity, IMultiTenant
{
    public Guid CompanyId { get; set; }

    public Guid WorkEventId { get; set; }
    public WorkEvent WorkEvent { get; set; } = null!;

    public Guid ActionPerformedByUserId { get; set; }
    public UserProfile ActionPerformedByUser { get; set; } = null!;

    public WorkEventStatus Status { get; set; } // The status of the work event at the time of this history entry.
    public DateTime ActionPerformedAt { get; set; } = DateTime.UtcNow;
    public string Description { get; set; } = string.Empty;
}
