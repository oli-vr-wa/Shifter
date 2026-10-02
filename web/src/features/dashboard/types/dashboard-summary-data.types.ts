import type { DashboardMetric } from "./dashboard-metric.types";

export interface DashboardSummaryData {
    jobs: DashboardMetric;
    hours: DashboardMetric;
    timesheets: DashboardMetric;
    employees: DashboardMetric;
}