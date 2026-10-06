import type { UpcomingJob } from "../types";
import { UpcomingJobItem } from "./UpcomingJobItem";

const mockUpcomingJobs: UpcomingJob[] = [
    {
        id: '1',
        title: "City Centre Electrical",
        date: "08/10/2026",
        employeesAssigned: 8,
        amount: 5850
    },
    {
        id: '2',
        title: "Northbridge Plumbing",
        date: "15/10/2026",
        employeesAssigned: 2,
        amount: 3200
    },
    {
        id: '3',
        title: "West End Carpentry",
        date: "22/10/2026",
        employeesAssigned: 5,
        amount: 4100
    }
]

export const UpcomingJobsWidget = () => {
    return (
        <div className="bg-white rounded-xl shadow-sm border border-slate-200">
            <div className="p-3 border-b border-slate-200">
                <h2 className="font-bold text-lg text-slate-900">
                    Upcoming Jobs
                </h2>
            </div>
            <div className="divide-y divide-slate-100">
                {mockUpcomingJobs.map(job => (
                    <UpcomingJobItem key={job.id} job={job} />
                ))}
            </div>
        </div>
    )
}