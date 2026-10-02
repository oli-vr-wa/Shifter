export type TimesheetStatus = 'approved' | 'pending' | 'rejected';

export interface TimesheetSummary {
    status: TimesheetStatus;
    count: number;
    percentage: number; // Percentage of the total timesheets with this status. 0 to 100.
}