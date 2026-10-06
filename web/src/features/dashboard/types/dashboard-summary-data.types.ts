import type { KPICardProps } from "@/components/ui/KPICard";

export interface DashboardSummaryData {
    jobs: KPICardProps;
    hours: KPICardProps;
    timesheets: KPICardProps;
    employees: KPICardProps;
}