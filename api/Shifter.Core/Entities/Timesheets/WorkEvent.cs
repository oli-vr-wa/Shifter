using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Tenant;

namespace Shifter.Core.Entities.Timesheets;

public class WorkEvent : BaseEntity
{
    public Guid EmployeeProfileId { get; set; }
    public UserProfile? EmployeeProfile { get; set; }
    public Guid CompanyId { get; set; }

    public WorkEventType Type { get; set; }

    // Schedule times to be used for planning and scheduling purposes. These times may not reflect the actual work performed.
    public DateTime? ScheduledStartTime { get; set; }
    public DateTime? ScheduledEndTime { get; set; }

    // Actual times to be used for tracking and reporting purposes. These times reflect the actual work performed.
    public DateTime? ActualStartTime { get; set; }
    public DateTime? ActualEndTime { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // TODO: Implement Address Location entity and add a foreign key relationship here for the location of the work event.

    public string? Notes { get; set; }
    public WorkEventStatus Status { get; set; }
}
