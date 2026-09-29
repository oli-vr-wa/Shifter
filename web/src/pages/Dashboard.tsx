import { Navigate, NavLink } from "react-router-dom";
import shifterLogo from "../assets/shifter-logo-v1.svg";
import { ClipboardDocumentCheckIcon, ClockIcon, ExclamationTriangleIcon, UsersIcon } from '@heroicons/react/24/outline';
import { ArrowLeftStartOnRectangleIcon, DocumentTextIcon, ClockIcon as ClockSolidIcon, CalendarIcon, Cog8ToothIcon, ChartBarIcon, UsersIcon as SolidUsersIcon } from '@heroicons/react/24/solid';
import { authService } from "@/features/auth";

export default function Dashboard() {

    const sidebarItemClassNames = "flex flex-row gap-2 hover:text-teal-400 hover:bg-teal-900 p-2 transition-colors duration-200 items-center cursor-pointer";

    const handleLogout = () => {
        authService.logout();
        return <Navigate to="/login" />;
    };

    return (
        <div className="flex min-h-screen">

            <nav className="md:fixed h-screen w-64 bg-teal-950 text-white text-sm overflow-auto z-1 flex flex-col">
                <div className="p-5 bg-[#011a1a] mb-2">
                    <img src={shifterLogo} alt="Shifter Logo" className="h-8 w-auto" />
                </div>
                <ul className="space-y-2">
                    <li>
                        <NavLink to="/dashboard" className={({ isActive }) => isActive ? `border-l-3 border-teal-400 text-teal-400 font-bold ${sidebarItemClassNames}` : sidebarItemClassNames}>
                            <ChartBarIcon className="size-5" />
                            Dashboard
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/employees" className={({ isActive }) => isActive ? `font-bold ${sidebarItemClassNames}` : sidebarItemClassNames}>
                            <CalendarIcon className="size-5" />
                            Schedule
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/employees" className={({ isActive }) => isActive ? `font-bold ${sidebarItemClassNames}` : sidebarItemClassNames}>
                            <ClockSolidIcon className="size-5" />
                            Timesheets
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/employees" className={({ isActive }) => isActive ? `font-bold ${sidebarItemClassNames}` : sidebarItemClassNames}>
                            <SolidUsersIcon className="size-5" />
                            Employees
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/employees" className={({ isActive }) => isActive ? `font-bold ${sidebarItemClassNames}` : sidebarItemClassNames}>
                            <DocumentTextIcon className="size-5" />
                            Reports
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/employees" className={({ isActive }) => isActive ? `font-bold ${sidebarItemClassNames}` : sidebarItemClassNames}>
                            <Cog8ToothIcon className="size-5" />
                            Settings
                        </NavLink>
                    </li>
                    <li>
                        <button onClick={handleLogout} className={` ${sidebarItemClassNames} w-full`}>
                            <ArrowLeftStartOnRectangleIcon className="size-5" />
                            Logout
                        </button>
                    </li>
                </ul>
                <div className="mt-auto flex shrink-0 p-4 bg-[#011a1a] gap-2 items-center">
                    <div className="w-10 h-10 rounded-full bg-teal-600 text-white flex items-center justify-center shrink-0">
                        OR
                    </div>
                    <div className="flex flex-col text-sm">
                        <span className="font-bold">Oliver Ramos</span>
                        <span className="text-teal-700">olirx.vr@gmail.com</span>
                    </div>
                    <button className="p-2 rounded-full hover:bg-teal-200">⋮</button>
                </div>
            </nav>
            
            <main className="flex-1 p-4 md:ml-64 bg-slate-50 min-h-screen">

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
                <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-4 mb-4">

                    <div className="bg-white rounded-xl shadow-sm border border-slate-200 p-5">
                        <div className="flex justify-between items-start">
                            <div>
                                <p className="text-sm text-slate-500">Jobs this week</p>
                                <p className="text-3xl font-bold text-slate-900 mt-1">24</p>
                            </div>

                            <div className="w-10 h-10 rounded-lg bg-teal-100 text-teal-700 flex items-center justify-center">
                                <ClipboardDocumentCheckIcon className="size-6" />
                            </div>
                        </div>

                        <p className="text-sm text-teal-600 mt-4">
                            ↑ 20% from last week
                        </p>
                    </div>


                    <div className="bg-white rounded-xl shadow-sm border border-slate-200 p-5">
                        <div className="flex justify-between items-start">
                            <div>
                                <p className="text-sm text-slate-500">Hours worked</p>
                                <p className="text-3xl font-bold text-slate-900 mt-1">342</p>
                            </div>

                            <div className="w-10 h-10 rounded-lg bg-blue-100 text-blue-700 flex items-center justify-center">
                                <ClockIcon className="size-6" />
                            </div>
                        </div>

                        <p className="text-sm text-blue-600 mt-4">
                            ↑ 8.4% from last week
                        </p>
                    </div>


                    <div className="bg-white rounded-xl shadow-sm border border-slate-200 p-5">
                        <div className="flex justify-between items-start">
                            <div>
                                <p className="text-sm text-slate-500">Timesheets pending</p>
                                <p className="text-3xl font-bold text-slate-900 mt-1">7</p>
                            </div>

                            <div className="w-10 h-10 rounded-lg bg-amber-100 text-amber-700 flex items-center justify-center">
                                <ExclamationTriangleIcon className="size-6" />
                            </div>
                        </div>

                        <p className="text-sm text-amber-600 mt-4">
                            Requires attention
                        </p>
                    </div>


                    <div className="bg-white rounded-xl shadow-sm border border-slate-200 p-5">
                        <div className="flex justify-between items-start">
                            <div>
                                <p className="text-sm text-slate-500">Employees working</p>
                                <p className="text-3xl font-bold text-slate-900 mt-1">18</p>
                            </div>

                            <div className="w-10 h-10 rounded-lg bg-purple-100 text-purple-700 flex items-center justify-center">
                                <UsersIcon className="size-6" />
                            </div>
                        </div>

                        <p className="text-sm text-slate-500 mt-4">
                            3 employees unavailable
                        </p>
                    </div>

                </div>


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

            </main>
        </div>
    );       
}