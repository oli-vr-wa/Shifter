namespace Shifter.Core.Entities.Timesheets.Enums;

public enum WorkEventStatus
{    
    // Admin creates or schedules a work event for a user
    Scheduled,
    // The work event has been created without being scheduled.
    Created,
    // User starts working on the work event
    Started,
    // User completes the work event and is ready for approval
    Completed,
    // Admin approves the work event
    Approved,
    // Admin rejects the work event
    Rejected,
    // Admin locks the work event to prevent further changes
    Locked
}

