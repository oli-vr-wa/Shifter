
namespace Shifter.Core.Entities.Timesheets;

public enum WorkEventStatus
{
    // Admin creates or schedules a work event for a user
    Scheduled,
    // User starts working on the work event
    InProgress,
    // User completes the work event and is ready for approval
    PendingApproval,
    // Admin approves the work event
    Approved,
    // Admin rejects the work event
    Rejected,
    // Admin locks the work event to prevent further changes
    Locked
}

