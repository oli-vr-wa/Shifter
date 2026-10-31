import { useState, useEffect } from 'react';

/**
 * Custom hook that returns a debounced version of a value.
 * The debounced value will only update after the specified delay has passed without the value changing.
 * @param value The value to debounce
 * @param delay The debounce delay in milliseconds (default: 300)
 * @returns The debounced value
 */

export const useDebouncedValue = <T>(value: T, delay = 300): T => {
    const [debounced, setDebounced] = useState(value);

    useEffect(() => {
        const timer = setTimeout(() => setDebounced(value), delay);
        return () => clearTimeout(timer);
    }, [value, delay]);

    return debounced;
};