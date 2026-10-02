import { Link } from "react-router-dom";
import type { JobSchedule } from "../types";
import { ScheduleListItem } from "./ScheduleListItem";

// Mock data - To be replaced with API call
const mockSchedule: JobSchedule[] = [
    {
        id: '1',
        startTime: '07:00 AM',
        endTime: '08:00 AM',
        title: 'Office Renovation',
        assignedTo: 'Oliver Valido',
        status: 'in-progress',
    },
    {
        id: '2',
        startTime: '9:00 AM',
        endTime: '3:00 PM',
        title: 'Warehouse Maintenance',
        assignedTo: 'Sarah Chen',
        status: 'starting-soon',
    },
    {
        id: '3',
        startTime: '1:00 PM',
        endTime: '5:30 PM',
        title: 'Retail Store Fitout',
        assignedTo: 'James Wilson',
        status: 'scheduled',
    },
    {
        id: '4',
        startTime: '2:00 PM',
        endTime: '4:00 PM',
        title: 'Client Meeting',
        assignedTo: 'Emily Davis',
        status: 'cancelled',
    }
];

export const TodaysScheduleWidget = () => {
    const today = new Date().toLocaleDateString('en-AU', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' });

    return (
        <div className="xl:col-span-2 bg-white rounded-xl shadow-sm border border-slate-200 flex flex-col h-full">
            <div className="p-4 border-b border-slate-200 flex justify-between items-center">
                <div>
                    <h2 className="font-bold text-lg text-slate-900">
                        Today's Schedule
                    </h2>
                    <p className="text-sm text-slate-500">
                        {today}
                    </p>
                </div>

                <Link to="/schedule" className="text-sm text-teal-700 hover:text-teal-900 font-medium">
                    View Schedule →
                </Link>
            </div>

            <div className="divide-y divide-slate-100 overflow-y-auto">
                {mockSchedule.map((job) => (
                    <ScheduleListItem key={job.id} job={job} />
                ))}

                {mockSchedule.length === 0 && (
                    <p className="p-8 text-center text-slate-500">
                        No scheduled jobs for today.
                    </p>
                )}
            </div>
        </div>
    )
}
