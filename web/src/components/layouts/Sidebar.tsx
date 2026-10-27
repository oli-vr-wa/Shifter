import shifterLogo from "@/assets/shifter-logo-v1.svg";
import { Navigate } from "react-router-dom";
import { ArrowLeftStartOnRectangleIcon, DocumentTextIcon, ClockIcon as ClockSolidIcon, CalendarIcon, Cog8ToothIcon, ChartBarIcon, UsersIcon as SolidUsersIcon, BriefcaseIcon, UserGroupIcon } from '@heroicons/react/24/solid';
import { useAuth } from "@/features/auth";
import { SidebarItem } from "./SidebarItem";

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
                    <SidebarItem
                        to="/dashboard"
                        label="Dashboard"
                        icon={<ChartBarIcon className="size-5" />}
                    />  
                    <SidebarItem
                        to="/jobs"
                        label="Jobs"
                        icon={<BriefcaseIcon className="size-5" />}
                    />
                    <SidebarItem
                        to="/schedule"
                        label="Schedule"
                        icon={<CalendarIcon className="size-5" />}
                    />
                    <SidebarItem
                        to="/timesheets"
                        label="Timesheets"
                        icon={<ClockSolidIcon className="size-5" />}
                    />
                    <SidebarItem
                        to="/employees"
                        label="Employees"
                        icon={<SolidUsersIcon className="size-5" />}
                    />
                    <SidebarItem
                        to="/clients"
                        label="Clients"
                        icon={<UserGroupIcon className="size-5" />}
                    />
                    <SidebarItem
                        to="/reports"
                        label="Reports"
                        icon={<DocumentTextIcon className="size-5" />}
                    />
                    <SidebarItem
                        to="/settings"
                        label="Settings"
                        icon={<Cog8ToothIcon className="size-5" />}
                    />
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