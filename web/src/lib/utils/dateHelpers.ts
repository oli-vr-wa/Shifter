/**
 * Formats a .NET ISO date string for an HTML <input type="date" />
 * Example: "2026-10-17T14:30:00Z" -> "2026-10-17"
 */
export const toInputDate = (isoString?: string | null): string => {
    if (!isoString) return '';
    return isoString.split('T')[0];
};

/**
 * Formats a .NET ISO date string for an HTML <input type="datetime-local" />
 * Example: "2026-10-17T14:30:00.123Z" -> "2026-10-17T14:30"
 */
export const toInputDateTime = (isoString?: string | null): string => {
    if (!isoString) return '';
    // Grabs exactly "YYYY-MM-DDTHH:mm" by slicing the first 16 characters
    return isoString.substring(0, 16);
};