import { NavLink } from "react-router-dom";

interface SidebarItemProps {
    to: string;
    label: string;
    icon: React.ReactNode;
}

export const SidebarItem = ({ to, label, icon }: SidebarItemProps) => {

    const sidebarItemClassNames = "flex flex-row gap-2 hover:text-teal-400 hover:bg-teal-900 p-2 transition-colors duration-200 items-center cursor-pointer";

    return (
        <li>
            <NavLink to={to} className={({ isActive }) => isActive 
                ? `border-l-3 border-teal-400 text-teal-400 font-bold ${sidebarItemClassNames}` 
                : sidebarItemClassNames}>
                {icon}
                {label}
            </NavLink>
        </li>
    );
};
