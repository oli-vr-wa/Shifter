import { act, renderHook, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { createElement, type ReactNode } from 'react';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { loadPlacesLibrary } from '@/lib/googleMaps';

// --- Mocks ---

const mocks = vi.hoisted(() => ({
    loadPlacesLibrary: vi.fn(),
    buildAddressFromPlace: vi.fn(),
    useDebouncedValue: vi.fn(),
    fetchAutocompleteSuggestions: vi.fn()
}));

vi.mock('@/lib/googleMaps', () => ({
    loadPlacesLibrary: mocks.loadPlacesLibrary
}));

vi.mock('./buildAddressFromPlace', () => ({
    PLACE_FIELDS: ['id', 'formattedAddress', 'addressComponents'],
    buildAddressFromPlace: mocks.buildAddressFromPlace
}));
    
vi.mock('@/shared/hooks/useDebouncedValue', () => ({
    useDebouncedValue: mocks.useDebouncedValue
}));

import { usePlacesAutocomplete, type SuggestionItem } from './usePlacesAutocomplete';

// --- Google Maps globals

class FakeSessionToken {
    static instances: FakeSessionToken[] = [];
    constructor() {
        FakeSessionToken.instances.push(this);
    }
}

// --- Helpers

const makeRawSuggestion = (placeId: string, label: string, place?: unknown) => ({
    placePrediction: {
        placeId,
        text: { toString: () => label },
        toPlace: vi.fn(() => place)
    }
});

const makeSuggestionItem = (placeId: string, label: string, place?: unknown): SuggestionItem => ({
    key: `${placeId}-${label}`,
    label,
    prediction: makeRawSuggestion(placeId, label, place).placePrediction as never
});

interface HookProps {
    query: string;
    isSelectedText?: boolean;
    regionCodes?: string[];
}

const setup = (initial: HookProps = { query: '' }) => {
    const queryClient = new QueryClient({
        defaultOptions: { queries: { retry: false, gcTime: Infinity } },
    });
    const wrapper = ({ children }: { children: ReactNode }) =>
        createElement(QueryClientProvider, { client: queryClient }, children);
 
    return renderHook(
        ({ query, isSelectedText = false, regionCodes = ['au'] }: HookProps) =>
            usePlacesAutocomplete({ query, isSelectedText, regionCodes }),
        { wrapper, initialProps: initial },
    );
};

/** Waits until the library has loaded and the first session exists. */
const waitForSession = async () => {
    await waitFor(() => expect(FakeSessionToken.instances.length).toBeGreaterThan(0));
};

// --- Tests

describe('usePlacesAutocomplete', () => {
    beforeEach(() => {
        FakeSessionToken.instances = [];
 
        mocks.loadPlacesLibrary.mockReset().mockResolvedValue(undefined);
        mocks.buildAddressFromPlace.mockReset();
        mocks.fetchAutocompleteSuggestions.mockReset().mockResolvedValue({ suggestions: [] });
        // Default: no debounce delay (identity).
        mocks.useDebouncedValue.mockReset().mockImplementation((value: string) => value);
 
        vi.stubGlobal('google', {
            maps: {
                places: {
                    AutocompleteSessionToken: FakeSessionToken,
                    AutocompleteSuggestion: {
                        fetchAutocompleteSuggestions: mocks.fetchAutocompleteSuggestions,
                    },
                },
            },
        });
    });

    afterEach(() => {
        vi.unstubAllGlobals();
    });

    describe('library loading & session', () => {
        it('loads the Places library once and creates a session when ready', async () => {
            setup();
 
            await waitForSession();
 
            expect(mocks.loadPlacesLibrary).toHaveBeenCalledTimes(1);
            expect(FakeSessionToken.instances).toHaveLength(1);
        });

        it('does not fetch suggestions before the library is ready', async () => {
            let resolveLibrary!: () => void;
            mocks.loadPlacesLibrary.mockReturnValue(new Promise<void>((r) => (resolveLibrary = r)));
 
            const { result } = setup({ query: 'sydney' });
 
            expect(mocks.fetchAutocompleteSuggestions).not.toHaveBeenCalled();
            expect(result.current.isSearching).toBe(false);
            expect(result.current.suggestions).toEqual([]);
 
            await act(async () => resolveLibrary());
            await waitFor(() => expect(mocks.fetchAutocompleteSuggestions).toHaveBeenCalled());
        });

        it('exposes the library error message when loading fails with an Error', async () => {
            mocks.loadPlacesLibrary.mockRejectedValue(new Error('Boom: script blocked'));
 
            const { result } = setup({ query: 'sydney' });
 
            await waitFor(() => expect(result.current.error).toBe('Boom: script blocked'));
            expect(mocks.fetchAutocompleteSuggestions).not.toHaveBeenCalled();
        });

        it('falls back to a generic message when the library error is not an Error', async () => {
            mocks.loadPlacesLibrary.mockRejectedValue('nope');
 
            const { result } = setup({ query: 'sydney' });
 
            await waitFor(() => expect(result.current.error).toBe('Could not load Google Maps library.'));
        });
    });

    describe('fetching suggestions', () => {
        it('does not search when the query is shorter than the minimum length', async () => {
            const { result } = setup({ query: 'a' });
            await waitForSession();
 
            expect(mocks.fetchAutocompleteSuggestions).not.toHaveBeenCalled();
            expect(result.current.suggestions).toEqual([]);
            expect(result.current.isSearching).toBe(false);
        });

        it('does not count surrounding whitespace towards the minimum length', async () => {
            setup({ query: ' a ' });
            await waitForSession();
 
            expect(mocks.fetchAutocompleteSuggestions).not.toHaveBeenCalled();
        });

        it('does not search when the text equals the already selected address', async () => {
            const { result } = setup({ query: '12 Hay St, Perth', isSelectedText: true });
            await waitForSession();
 
            expect(mocks.fetchAutocompleteSuggestions).not.toHaveBeenCalled();
            expect(result.current.suggestions).toEqual([]);
            expect(result.current.isSearching).toBe(false);
        });

        it('calls Google with the trimmed query, session token and region codes', async () => {
            setup({ query: '  12 Hay St  ', regionCodes: ['au', 'nz'] });
 
            await waitFor(() => expect(mocks.fetchAutocompleteSuggestions).toHaveBeenCalledTimes(1));
 
            expect(mocks.fetchAutocompleteSuggestions).toHaveBeenCalledWith({
                input: '12 Hay St',
                sessionToken: FakeSessionToken.instances[0],
                includedRegionCodes: ['au', 'nz'],
            });
        });

        it('maps predictions to suggestion items with a stable key', async () => {
            mocks.fetchAutocompleteSuggestions.mockResolvedValue({
                suggestions: [makeRawSuggestion('p1', '1 Main St, Perth'), makeRawSuggestion('p2', '2 Main St, Perth')],
            });
 
            const { result } = setup({ query: 'main st' });
 
            await waitFor(() => expect(result.current.suggestions).toHaveLength(2));
 
            expect(result.current.suggestions.map(({ key, label }) => ({ key, label }))).toEqual([
                { key: 'p1-1 Main St, Perth', label: '1 Main St, Perth' },
                { key: 'p2-2 Main St, Perth', label: '2 Main St, Perth' },
            ]);
            expect(result.current.suggestions[0].prediction).toBeDefined();
        });

        it('drops suggestions without a placePrediction or without label text', async () => {
            mocks.fetchAutocompleteSuggestions.mockResolvedValue({
                suggestions: [
                    { placePrediction: null }, // query prediction, not a place
                    makeRawSuggestion('p-empty', ''),
                    makeRawSuggestion('p-ok', 'Valid Place'),
                ],
            });
 
            const { result } = setup({ query: 'valid' });
 
            await waitFor(() => expect(result.current.suggestions).toHaveLength(1));
            expect(result.current.suggestions[0].label).toBe('Valid Place');
        });

        it('reports isSearching while the request is in flight, then clears it', async () => {
            let resolveFetch!: (v: unknown) => void;
            mocks.fetchAutocompleteSuggestions.mockReturnValue(new Promise((r) => (resolveFetch = r)));
 
            const { result } = setup({ query: 'perth' });
 
            await waitFor(() => expect(result.current.isSearching).toBe(true));
 
            await act(async () => resolveFetch({ suggestions: [makeRawSuggestion('p1', 'Perth WA')] }));
 
            await waitFor(() => expect(result.current.isSearching).toBe(false));
            expect(result.current.suggestions).toHaveLength(1);
        });

        it('sets an error when fetching suggestions fails', async () => {
            mocks.fetchAutocompleteSuggestions.mockRejectedValue(new Error('network'));
 
            const { result } = setup({ query: 'perth' });
 
            await waitFor(() =>
                expect(result.current.error).toBe('Unable to fetch address suggestions. Keep typing to try again.'),
            );
        });

        it('hides suggestions and the suggestion error once the input is no longer searchable', async () => {
            mocks.fetchAutocompleteSuggestions.mockResolvedValue({
                suggestions: [makeRawSuggestion('p1', 'Perth WA')],
            });
 
            const { result, rerender } = setup({ query: 'perth' });
            await waitFor(() => expect(result.current.suggestions).toHaveLength(1));
 
            rerender({ query: 'perth', isSelectedText: true });
 
            expect(result.current.suggestions).toEqual([]);
            expect(result.current.isSearching).toBe(false);
        });
    });

    describe('debouncing', () => {
        it('does not fetch while the debounced value lags behind the typed value', async () => {
            // Simulate a debounce that has not yet caught up.
            mocks.useDebouncedValue.mockImplementation(() => 'per');
 
            const { result } = setup({ query: 'perth' });
            await waitForSession();
 
            expect(mocks.fetchAutocompleteSuggestions).not.toHaveBeenCalled();
            expect(result.current.isSearching).toBe(true);
        });
 
        it('passes the trimmed query and delay to useDebouncedValue (default 300ms)', async () => {
            setup({ query: '  perth ' });
            await waitForSession();
 
            expect(mocks.useDebouncedValue).toHaveBeenCalledWith('perth', 300);
        });
 
        it('fetches with the debounced value once it catches up', async () => {
            let debounced = 'per';
            mocks.useDebouncedValue.mockImplementation(() => debounced);
 
            const { rerender } = setup({ query: 'perth' });
            await waitForSession();
            expect(mocks.fetchAutocompleteSuggestions).not.toHaveBeenCalled();
 
            debounced = 'perth';
            rerender({ query: 'perth' });
 
            await waitFor(() => expect(mocks.fetchAutocompleteSuggestions).toHaveBeenCalledTimes(1));
            expect(mocks.fetchAutocompleteSuggestions).toHaveBeenCalledWith(
                expect.objectContaining({ input: 'perth' }),
            );
        });
    });
 
    describe('selecting a suggestion', () => {
        const address = { street: '1 Main St', suburb: 'Perth', state: 'WA', postcode: '6000' };
 
        it('fetches place fields, builds the address and calls onSuccess', async () => {
            const place = { fetchFields: vi.fn().mockResolvedValue(undefined) };
            mocks.buildAddressFromPlace.mockReturnValue(address);
            const suggestion = makeSuggestionItem('p1', '1 Main St, Perth', place);
            const onSuccess = vi.fn();
 
            const { result } = setup();
            await waitForSession();
 
            act(() => result.current.selectSuggestion(suggestion, onSuccess));
 
            await waitFor(() => expect(onSuccess).toHaveBeenCalledTimes(1));
 
            expect(place.fetchFields).toHaveBeenCalledWith({
                fields: ['id', 'formattedAddress', 'addressComponents'],
            });
            expect(mocks.buildAddressFromPlace).toHaveBeenCalledWith(place);
            expect(onSuccess.mock.calls[0][0]).toEqual(address);
            expect(result.current.error).toBeNull();
        });
 
        it('reports isSelecting while place details are loading', async () => {
            let resolveFields!: () => void;
            const place = { fetchFields: vi.fn(() => new Promise<void>((r) => (resolveFields = r))) };
            mocks.buildAddressFromPlace.mockReturnValue(address);
 
            const { result } = setup();
            await waitForSession();
 
            act(() => result.current.selectSuggestion(makeSuggestionItem('p1', 'X', place), vi.fn()));
 
            await waitFor(() => expect(result.current.isSelecting).toBe(true));
 
            await act(async () => resolveFields());
            await waitFor(() => expect(result.current.isSelecting).toBe(false));
        });
 
        it('starts a new billing session after a successful selection', async () => {
            const place = { fetchFields: vi.fn().mockResolvedValue(undefined) };
            mocks.buildAddressFromPlace.mockReturnValue(address);
 
            const { result } = setup();
            await waitForSession();
            expect(FakeSessionToken.instances).toHaveLength(1);
 
            const onSuccess = vi.fn();
            act(() => result.current.selectSuggestion(makeSuggestionItem('p1', 'X', place), onSuccess));
 
            await waitFor(() => expect(onSuccess).toHaveBeenCalled());
            await waitFor(() => expect(FakeSessionToken.instances).toHaveLength(2));
        });
 
        it('sets an error and skips onSuccess when the place cannot be mapped to an address', async () => {
            const place = { fetchFields: vi.fn().mockResolvedValue(undefined) };
            mocks.buildAddressFromPlace.mockReturnValue(null);
            const onSuccess = vi.fn();
 
            const { result } = setup();
            await waitForSession();
 
            act(() => result.current.selectSuggestion(makeSuggestionItem('p1', 'X', place), onSuccess));
 
            await waitFor(() =>
                expect(result.current.error).toBe('Unable to fetch the selected address details.'),
            );
            expect(onSuccess).not.toHaveBeenCalled();
            // Session must be kept so the user can retry within the same billing session.
            expect(FakeSessionToken.instances).toHaveLength(1);
        });
 
        it('sets an error when fetchFields rejects', async () => {
            const place = { fetchFields: vi.fn().mockRejectedValue(new Error('403')) };
 
            const { result } = setup();
            await waitForSession();
 
            act(() => result.current.selectSuggestion(makeSuggestionItem('p1', 'X', place), vi.fn()));
 
            await waitFor(() =>
                expect(result.current.error).toBe('Unable to fetch the selected address details.'),
            );
        });
 
        it('clearSelectError removes the selection error', async () => {
            const place = { fetchFields: vi.fn().mockRejectedValue(new Error('403')) };
 
            const { result } = setup();
            await waitForSession();
 
            act(() => result.current.selectSuggestion(makeSuggestionItem('p1', 'X', place), vi.fn()));
            await waitFor(() => expect(result.current.error).not.toBeNull());
 
            act(() => result.current.clearSelectError());
 
            await waitFor(() => expect(result.current.error).toBeNull());
        });
 
        it('prioritises the library error over selection and suggestion errors', async () => {
            mocks.loadPlacesLibrary.mockRejectedValue(new Error('library down'));
 
            const { result } = setup({ query: 'perth' });
 
            await waitFor(() => expect(result.current.error).toBe('library down'));
        });
    });
 
    describe('resetSession', () => {
        it('creates a fresh session token', async () => {
            const { result } = setup();
            await waitForSession();
            expect(FakeSessionToken.instances).toHaveLength(1);
 
            act(() => result.current.resetSession());
 
            expect(FakeSessionToken.instances).toHaveLength(2);
        });
 
        it('re-fetches suggestions with the new token (session id is part of the cache key)', async () => {
            mocks.fetchAutocompleteSuggestions.mockResolvedValue({
                suggestions: [makeRawSuggestion('p1', 'Perth WA')],
            });
 
            const { result } = setup({ query: 'perth' });
            await waitFor(() => expect(mocks.fetchAutocompleteSuggestions).toHaveBeenCalledTimes(1));
 
            const firstToken = mocks.fetchAutocompleteSuggestions.mock.calls[0][0].sessionToken;
 
            act(() => result.current.resetSession());
 
            await waitFor(() => expect(mocks.fetchAutocompleteSuggestions).toHaveBeenCalledTimes(2));
 
            const secondToken = mocks.fetchAutocompleteSuggestions.mock.calls[1][0].sessionToken;
            expect(secondToken).not.toBe(firstToken);
        });
    });
 
    describe('caching', () => {
        it('does not refetch when re-rendered with the same query, regions and session', async () => {
            mocks.fetchAutocompleteSuggestions.mockResolvedValue({
                suggestions: [makeRawSuggestion('p1', 'Perth WA')],
            });
 
            const { result, rerender } = setup({ query: 'perth', regionCodes: ['au'] });
            await waitFor(() => expect(result.current.suggestions).toHaveLength(1));
 
            rerender({ query: 'perth', regionCodes: ['au'] });
 
            expect(mocks.fetchAutocompleteSuggestions).toHaveBeenCalledTimes(1);
        });
 
        it('refetches when the region codes change', async () => {
            const { rerender } = setup({ query: 'perth', regionCodes: ['au'] });
            await waitFor(() => expect(mocks.fetchAutocompleteSuggestions).toHaveBeenCalledTimes(1));
 
            rerender({ query: 'perth', regionCodes: ['nz'] });
 
            await waitFor(() => expect(mocks.fetchAutocompleteSuggestions).toHaveBeenCalledTimes(2));
            expect(mocks.fetchAutocompleteSuggestions).toHaveBeenLastCalledWith(
                expect.objectContaining({ includedRegionCodes: ['nz'] }),
            );
        });
    });
});