import { DashboardMetrics } from '@/features/dashboard';

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
                <div className="xl:col-span-2 bg-white rounded-xl shadow-sm border border-slate-200">

                    <div className="p-3 border-b border-slate-200 flex justify-between items-center">
                        <div>
                            <h2 className="font-bold text-lg text-slate-900">
                                Today's Schedule
                            </h2>
                            <p className="text-sm text-slate-500">
                                Tuesday, 25 August
                            </p>
                        </div>

                        <button className="text-sm text-teal-700 hover:text-teal-900 font-medium">
                            View schedule →
                        </button>
                    </div>


                    <div className="divide-y divide-slate-100">

                        <div className="p-3 flex items-center gap-4 hover:bg-slate-50">

                            <div className="w-16 text-sm text-slate-500">
                                7:00 AM
                            </div>

                            <div className="w-1 h-12 bg-teal-500 rounded-full"></div>

                            <div className="flex-1">
                                <p className="font-semibold text-slate-900">
                                    Office Renovation
                                </p>
                                <p className="text-sm text-slate-500">
                                    Oliver Ramos · 7:00 AM – 11:00 AM
                                </p>
                            </div>

                            <span className="px-3 py-1 text-xs font-medium rounded-full bg-green-100 text-green-700">
                                In progress
                            </span>

                        </div>


                        <div className="p-3 flex items-center gap-4 hover:bg-slate-50">

                            <div className="w-16 text-sm text-slate-500">
                                9:00 AM
                            </div>

                            <div className="w-1 h-12 bg-amber-500 rounded-full"></div>                                

                            <div className="flex-1">
                                <p className="font-semibold text-slate-900">
                                    Warehouse Maintenance
                                </p>
                                <p className="text-sm text-slate-500">
                                    Sarah Chen · 9:00 AM – 3:00 PM
                                </p>
                            </div>

                            <span className="px-3 py-1 text-xs font-medium rounded-full bg-amber-100 text-amber-700">
                                Starting soon
                            </span>

                        </div>


                        <div className="p-3 flex items-center gap-4 hover:bg-slate-50">

                            <div className="w-16 text-sm text-slate-500">
                                1:00 PM
                            </div>

                            <div className="w-1 h-12 bg-blue-500 rounded-full"></div>

                            <div className="flex-1">
                                <p className="font-semibold text-slate-900">
                                    Retail Store Fitout
                                </p>
                                <p className="text-sm text-slate-500">
                                    James Wilson · 1:00 PM – 5:30 PM
                                </p>
                            </div>

                            <span className="px-3 py-1 text-xs font-medium rounded-full bg-blue-100 text-blue-700">
                                Scheduled
                            </span>

                        </div>

                        <div className="p-3 flex items-center gap-4 hover:bg-slate-50">

                            <div className="w-16 text-sm text-slate-500">
                                2:00 PM
                            </div>

                            <div className="w-1 h-12 bg-blue-500 rounded-full"></div>

                            <div className="flex-1">
                                <p className="font-semibold text-slate-900">
                                    Shower Installation
                                </p>
                                <p className="text-sm text-slate-500">
                                    John Doe · 2:00 PM – 3:30 PM
                                </p>
                            </div>

                            <span className="px-3 py-1 text-xs font-medium rounded-full bg-blue-100 text-blue-700">
                                Scheduled
                            </span>

                        </div>

                    </div>

                </div>


                {/* Timesheet status */}
                <div className="bg-white rounded-xl shadow-sm border border-slate-200">

                    <div className="p-3 border-b border-slate-200">
                        <h2 className="font-bold text-lg text-slate-900">
                            Timesheets
                        </h2>

                        <p className="text-sm text-slate-500">
                            Current approval status
                        </p>
                    </div>


                    <div className="p-5 space-y-5">

                        <div>
                            <div className="flex justify-between mb-2">
                                <span className="text-sm text-slate-600">
                                    Approved
                                </span>
                                <span className="text-sm font-semibold">
                                    32
                                </span>
                            </div>

                            <div className="h-2 bg-slate-100 rounded-full">
                                <div className="h-2 bg-teal-500 rounded-full w-[78%]"></div>
                            </div>
                        </div>


                        <div>
                            <div className="flex justify-between mb-2">
                                <span className="text-sm text-slate-600">
                                    Pending
                                </span>
                                <span className="text-sm font-semibold">
                                    7
                                </span>
                            </div>

                            <div className="h-2 bg-slate-100 rounded-full">
                                <div className="h-2 bg-amber-400 rounded-full w-[18%]"></div>
                            </div>
                        </div>


                        <div>
                            <div className="flex justify-between mb-2">
                                <span className="text-sm text-slate-600">
                                    Rejected
                                </span>
                                <span className="text-sm font-semibold">
                                    2
                                </span>
                            </div>

                            <div className="h-2 bg-slate-100 rounded-full">
                                <div className="h-2 bg-red-400 rounded-full w-[5%]"></div>
                            </div>
                        </div>


                        <button className="w-full mt-2 py-2 rounded-lg bg-teal-600 text-white text-sm font-medium hover:bg-teal-700">
                            Review timesheets
                        </button>

                    </div>

                </div>

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