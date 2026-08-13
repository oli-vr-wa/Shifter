using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Location;
using Shifter.Core.Entities.Tenant;
using Shifter.Core.Entities.Timesheets.Enums;

namespace Shifter.Core.Entities.Timesheets;

public class WorkEvent : BaseEntity, IMultiTenant
{
    public Guid UserProfileId { get; set; }
    public UserProfile? UserProfile { get; set; }
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

    public int? AddressId { get; set; }
    public Address? Address { get; set; }

    public string? Notes { get; set; }
    public WorkEventStatus Status { get; set; }

    // Tracking who assigned the work event and who created it. 
    public Guid? AssignedByUserId { get; set; } // If null means the work event was created by the employee themselves.
    public UserProfile? AssignedByUser { get; set; }

    public Guid CreatedByUserId { get; set; }
    public UserProfile CreatedByUser { get; set; } = null!;
}
