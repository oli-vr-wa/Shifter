import { DashboardMetrics } from '@/features/dashboard';
import { TodaysScheduleWidget } from '@/features/schedule';
import { TimesheetsOverviewWidget } from '@/features/timesheets';

export default function DashboardPage() {

    return (            
        <>

            {/* Header */}
            <div className="mb-6">
                <h1 className="text-2xl font-bold text-slate-900">
                    Dashboard
                </h1>
                <p className="text-sm text-slate-500">
                    Here's what's happening with your jobs and employees today.
                </p>
            </div>


            {/* KPI Cards */}  
            <DashboardMetrics />

            {/* Main content */}
            <div className="grid grid-cols-1 xl:grid-cols-3 gap-4">

                {/* Today's schedule */}
                <TodaysScheduleWidget />

                {/* Timesheet status */}
                <TimesheetsOverviewWidget />

            </div>


            {/* Bottom section */}
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-4 mt-4">


                {/* Upcoming jobs */}
                <div className="bg-white rounded-xl shadow-sm border border-slate-200">

                    <div className="p-3 border-b border-slate-200">
                        <h2 className="font-bold text-lg text-slate-900">
                            Upcoming Jobs
                        </h2>
                    </div>

                    <div className="divide-y divide-slate-100">

                        <div className="p-3 flex justify-between">
                            <div>
                                <p className="font-medium text-slate-900">
                                    City Centre Electrical
                                </p>
                                <p className="text-sm text-slate-500">
                                    Tomorrow · 8 employees
                                </p>
                            </div>

                            <span className="text-sm text-teal-600">
                                $4,850
                            </span>
                        </div>

                        <div className="p-3 flex justify-between">
                            <div>
                                <p className="font-medium text-slate-900">
                                    Northbridge Construction
                                </p>
                                <p className="text-sm text-slate-500">
                                    Thu 27 Aug · 12 employees
                                </p>
                            </div>

                            <span className="text-sm text-teal-600">
                                $7,240
                            </span>
                        </div>

                        <div className="p-3 flex justify-between">
                            <div>
                                <p className="font-medium text-slate-900">
                                    Belmont Warehouse
                                </p>
                                <p className="text-sm text-slate-500">
                                    Fri 28 Aug · 6 employees
                                </p>
                            </div>

                            <span className="text-sm text-teal-600">
                                $2,960
                            </span>
                        </div>

                    </div>

                </div>


                {/* Activity */}
                <div className="bg-white rounded-xl shadow-sm border border-slate-200">

                    <div className="p-3 border-b border-slate-200">
                        <h2 className="font-bold text-lg text-slate-900">
                            Recent Activity
                        </h2>
                    </div>

                    <div className="p-5 space-y-5">

                        <div className="flex gap-3">
                            <div className="w-8 h-8 rounded-full bg-teal-100 text-teal-700 flex items-center justify-center shrink-0">
                                OR
                            </div>

                            <div>
                                <p className="text-sm text-slate-700">
                                    <strong>Oliver Ramos</strong> approved a timesheet
                                </p>
                                <p className="text-xs text-slate-400 mt-1">
                                    10 minutes ago
                                </p>
                            </div>
                        </div>


                        <div className="flex gap-3">
                            <div className="w-8 h-8 rounded-full bg-blue-100 text-blue-700 flex items-center justify-center shrink-0">
                                SC
                            </div>

                            <div>
                                <p className="text-sm text-slate-700">
                                    <strong>Sarah Chen</strong> submitted a timesheet
                                </p>
                                <p className="text-xs text-slate-400 mt-1">
                                    32 minutes ago
                                </p>
                            </div>
                        </div>


                        <div className="flex gap-3">
                            <div className="w-8 h-8 rounded-full bg-purple-100 text-purple-700 flex items-center justify-center shrink-0">
                                JW
                            </div>

                            <div>
                                <p className="text-sm text-slate-700">
                                    <strong>James Wilson</strong> was assigned to a job
                                </p>
                                <p className="text-xs text-slate-400 mt-1">
                                    1 hour ago
                                </p>
                            </div>
                        </div>

                    </div>

                </div>

            </div>

        </>
    );       
}