import type { MetricTrend } from "../types";

/**
 * Formats a trend string with an optional directional symbol.
 * @param trendData - The trend data containing the trend string and direction.
 * @returns The formatted trend string with a symbol, or undefined if no trend data is provided.
 */
export const formatTrendString = (trendData?: MetricTrend): string | undefined => {
    if (!trendData) return undefined;

    const { trend, direction } = trendData;

    const symbols: Record<MetricTrend['direction'], string> = {
        up: '↑',
        down: '↓',
        neutral: ''
    };

    const symbol = symbols[direction];

    // Prepend the symbol to the trend string and trim any extra whitespace
    // Example: "↑ 10% increase" or "↓ 5% decrease"
    return `${symbol} ${trend}`.trim();
};