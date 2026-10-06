import type { UpcomingJob } from '../types';
import { formatCurrency } from '@/lib/utils';

interface UpcomingJobItemProps {
    job: UpcomingJob;
}

export const UpcomingJobItem = ({ job }: UpcomingJobItemProps) => {
    return (
        <div className="p-3 flex justify-between">
            <div>
                <p className="font-medium text-slate-900">
                    {job.title}
                </p>
                <p className="text-sm text-slate-500">
                    {job.date} · {job.employeesAssigned} employees
                </p>
            </div>

            <span className="text-sm text-teal-600">
                {formatCurrency(job.amount)}
            </span>
        </div>
    );
};
