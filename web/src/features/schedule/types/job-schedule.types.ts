export type JobStatus = 'in-progress' | 'starting-soon' | 'scheduled' | 'completed' | 'cancelled';

export interface JobSchedule {
    id: string;
    startTime: string;
    endTime: string;
    title: string;
    assignedTo: string;
    status: JobStatus;
}