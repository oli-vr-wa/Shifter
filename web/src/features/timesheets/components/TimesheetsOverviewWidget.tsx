import { Button } from "@/components";
import { TimesheetStatusRow } from "./TimesheetStatusRow";
import type { TimesheetSummary } from "../types";

const mockTimesheetSummaries: TimesheetSummary[] = [
    { id: '1', status: 'approved', count: 5, percentage: 50 },
    { id: '2', status: 'pending', count: 3, percentage: 30 },
    { id: '3', status: 'rejected', count: 2, percentage: 20 },
];

export const TimesheetsOverviewWidget = () => {
    return (
        <div className="bg-white rounded-xl shadow-sm border border-slate-200">

            <div className="p-3 border-b border-slate-200">
                <h2 className="text-lg font-bold text-slate-900">Timesheets Overview</h2>
                <p className="text-sm text-slate-500">Current approval status</p>
            </div>

            <div className="p-5 space-y-5">

                {mockTimesheetSummaries.map((summary) => (
                    <TimesheetStatusRow key={summary.id} summary={summary} />
                ))}

                <Button className="w-full text-sm">Review Timesheets</Button>
            </div>

        </div>
    )
}