import type { JobSchedule } from '../types';

interface ScheduleListItemProps {
    job: JobSchedule;
}

const statusConfig = {
    'in-progress': { label: 'In Progress', line: 'bg-teal-500', badge: 'bg-teal-100 text-teal-700' },
    'starting-soon': { label: 'Starting Soon', line: 'bg-amber-500', badge: 'bg-amber-100 text-amber-700' },
    'scheduled': { label: 'Scheduled', line: 'bg-blue-500', badge: 'bg-blue-100 text-blue-700' },
    'completed': { label: 'Completed', line: 'bg-purple-500', badge: 'bg-purple-100 text-purple-700' },
    'cancelled': { label: 'Cancelled', line: 'bg-red-500', badge: 'bg-red-100 text-red-700' },
};

export const ScheduleListItem = ({ job }: ScheduleListItemProps) => {
    const theme = statusConfig[job.status];

    return (
        <div className="p-3 flex items-center gap-4 hover:bg-slate-50 transition-colors duration-200">
            <div className="w-16 text-sm text-slate-500 whitespace-nowrap">
                {job.startTime}
            </div>

            <div className={`w-1 h-12 rounded-full ${theme.line}`}></div>

            <div className="flex-1">
                <p className="font-semibold text-slate-900">
                    {job.title}
                </p>
                <p className="text-sm text-slate-500">
                    {job.assignedTo} · {job.startTime} - {job.endTime}
                </p>
            </div>

            <span className={`px-3 py-1 text-xs font-medium rounded-full ${theme.badge}`}>
                {theme.label}
            </span>
        </div>
    );
}