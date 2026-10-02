import shifterLogo from "@/assets/shifter-logo-v1.svg";
import { Navigate, NavLink } from "react-router-dom";
import { ArrowLeftStartOnRectangleIcon, DocumentTextIcon, ClockIcon as ClockSolidIcon, CalendarIcon, Cog8ToothIcon, ChartBarIcon, UsersIcon as SolidUsersIcon } from '@heroicons/react/24/solid';
import { useAuth } from "@/features/auth";

export const Sidebar = () => {
    const { logout } = useAuth();

    const sidebarItemClassNames = "flex flex-row gap-2 hover:text-teal-400 hover:bg-teal-900 p-2 transition-colors duration-200 items-center cursor-pointer";

    const handleLogout = () => {
        logout();
        return <Navigate to="/login" />;
    };

    return (
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
    );
};