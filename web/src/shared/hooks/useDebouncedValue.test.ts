import { act, renderHook } from '@testing-library/react';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { useDebouncedValue } from './useDebouncedValue';

describe('useDebouncedValue', () => {
    // Fake timers let us "wait" 300ms instantly and deterministically
    // This replaces setTimeout and clearTimeout with fake timers.
    beforeEach(() => {
        vi.useFakeTimers();
    });

    afterEach(() => {
        vi.useRealTimers();
    });

    const setup = (initial: string) =>
        renderHook(({ value }) => useDebouncedValue(value, 300), {
            initialProps: { value: initial }
        });

    it('returns the initial value immediately', () => {
        const { result } = setup('a');
        expect(result.current).toBe('a');
    });

    it('only updates after the delay has passed', () => {
        const { result, rerender } = setup('a');

        rerender({ value: 'ab' });
        expect(result.current).toBe('a');

        act(() => {
            vi.advanceTimersByTime(299);
        });
        expect(result.current).toBe('a');

        act(() => {
            vi.advanceTimersByTime(1);
        });
        expect(result.current).toBe('ab');
    });

    it('restarts the timer when the value changes again', () => {
        const { result, rerender } = setup('a');

        rerender({ value: 'ab' });
        act(() => {
            vi.advanceTimersByTime(200);
        });

        rerender({ value: 'abc' });
        act(() => {
            vi.advanceTimersByTime(200); // 400 ms since the first change, and 200 ms since the second change
        });
        expect(result.current).toBe('a');

        act(() => {
            vi.advanceTimersByTime(100); // 300 ms since the second change
        });
        expect(result.current).toBe('abc'); // 'ab' was skipped entirely.
    })
});