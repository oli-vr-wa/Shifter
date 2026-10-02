import type { TimesheetSummary } from '../types';

interface TimesheetStatusRowProps {
    summary: TimesheetSummary;
}

const themeConfig = {
    'approved': { label: 'Approved', colorClass: 'bg-teal-500' },
    'pending': { label: 'Pending', colorClass: 'bg-amber-400' },
    'rejected': { label: 'Rejected', colorClass: 'bg-red-400' },
}

export function TimesheetStatusRow({ summary }: TimesheetStatusRowProps) {
    const theme = themeConfig[summary.status];

    return (
        <>
            <div className="flex justify-between mb-2">
                <span className="text-sm text-slate-600">
                    {theme.label}
                </span>
                <span className="text-sm font-semibold">
                    {summary.count}
                </span>
            </div>
            <div className="h-2 bg-slate-100 rounded-full overflow-hidden">
                <div className={`h-full rounded-full transition-all duration-500 ${theme.colorClass}`} style={{ width: `${summary.percentage}%` }}></div>
            </div>
        </>
    );
}
