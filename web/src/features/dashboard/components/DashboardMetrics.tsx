import { useEffect, useState } from "react";
import type { DashboardSummaryData } from "../types";
import { KPICard } from "@/components/ui/KPICard";
import { ClipboardDocumentCheckIcon, ClockIcon, ExclamationTriangleIcon, UsersIcon } from "@heroicons/react/24/outline";

/**
 * DashboardMetrics component displays key performance indicators (KPIs) for the dashboard.
 * It fetches and displays summary data for jobs, hours, timesheets, and employees.
 */
export const DashboardMetrics = () => {
    const [data, setData] = useState<DashboardSummaryData | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(true);

    // Simulate fetching dashboard summary data
    // This could be replaced with an actual API call
    useEffect(() => {
        
        const mockApiResponse: DashboardSummaryData = {
            jobs: {
                title: "Jobs",
                value: 24,
                trend: {
                    direction: 'up',
                    trend: '20 % from last week'
                }
            },
            hours: {
                title: "Hours",
                value: 345,
                trend: {
                    direction: 'up',
                    trend: '9.2 % from last week'
                }
            },
            timesheets: {
                title: "Timesheets",
                value: 7,
                trend: {
                    direction: 'neutral',
                    trend: 'Requires attention'
                }
            },
            employees: {
                title: "Employees",
                value: 8,
                trend: {
                    direction: 'neutral',
                    trend: '2 employees available'
                }
            }
        };
        setData(mockApiResponse);
        setIsLoading(false);
    }, []);

    return (
        <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-4 mb-4">            
            <KPICard
                title={data?.jobs.title ?? ""}
                value={data?.jobs.value ?? 0}
                themeColor={'green'}
                trend={data?.jobs.trend}
                icon={<ClipboardDocumentCheckIcon className="size-6" />}
                isLoading={isLoading}
            />
            <KPICard
                title={data?.hours.title ?? ""}
                value={data?.hours.value ?? 0}
                themeColor={'blue'}
                trend={data?.hours.trend}
                icon={<ClockIcon className="size-6" />}
                isLoading={isLoading}
            />
            <KPICard
                title={data?.timesheets.title ?? ""}
                value={data?.timesheets.value ?? 0}
                themeColor={'yellow'}
                trend={data?.timesheets.trend}
                icon={<ExclamationTriangleIcon className="size-6" />}
                isLoading={isLoading}
            />
            <KPICard
                title={data?.employees.title ?? ""}
                value={data?.employees.value ?? 0}
                themeColor={'purple'}
                trend={data?.employees.trend}
                icon={<UsersIcon className="size-6" />}
                isLoading={isLoading}
            />            
        </div>
    );
};