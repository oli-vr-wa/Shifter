import { Outlet } from "react-router-dom";
import { Sidebar } from "@/components/Sidebar";

export const AppLayout = () => {
    return (
        <div className="flex min-h-screen">

            <Sidebar />

            <main className="flex-1 p-4 md:ml-64 bg-slate-50 min-h-screen">
                <Outlet />
            </main>

        </div>
    );

}