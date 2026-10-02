import type { MetricTrend } from "./metric-trend.types";

export interface DashboardMetric {
    title: string;
    currentValue: string | number;
    trend?: MetricTrend;
}