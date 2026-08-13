using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Location;
using Shifter.Core.Entities.Tenant;
using Shifter.Core.Entities.Timesheets.Enums;

namespace Shifter.Core.Entities.Timesheets;

public class WorkEventHistory : BaseEntity, IMultiTenant
{
    public Guid CompanyId { get; set; }

    // The work event or break event that this history entry is associated with. Only one of these should be set for a given history entry.
    public Guid? WorkEventId { get; set; }
    public WorkEvent? WorkEvent { get; set; }
    public Guid? BreakEventId { get; set; }
    public BreakEvent? BreakEvent { get; set; }

    public Guid ActionPerformedByUserId { get; set; }
    public UserProfile ActionPerformedByUser { get; set; } = null!;

    public WorkEventStatus Status { get; set; } // The status of the work event at the time of this history entry.
    public DateTime ActionPerformedAt { get; set; } = DateTime.UtcNow;
    public int? ActionPerformedAtAddressId { get; set; }
    public Address? ActionPerformedAtAddress { get; set; }
    public string Description { get; set; } = string.Empty;
}
