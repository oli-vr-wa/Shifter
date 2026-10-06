import { Badge } from '@/components/ui/Badge';
import type { JobSchedule } from '../types';

interface ScheduleListItemProps {
    job: JobSchedule;
}

const statusConfig = {
    'in-progress': { label: 'In Progress', line: 'bg-teal-500', badgeType: 'success' },
    'starting-soon': { label: 'Starting Soon', line: 'bg-amber-500', badgeType: 'warning' },
    'scheduled': { label: 'Scheduled', line: 'bg-blue-500', badgeType: 'default' },
    'completed': { label: 'Completed', line: 'bg-purple-500', badgeType: 'info' },
    'cancelled': { label: 'Cancelled', line: 'bg-red-500', badgeType: 'error' },
} as const;

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

            <Badge type={theme.badgeType}>
                {theme.label}
            </Badge>
        </div>
    );
}