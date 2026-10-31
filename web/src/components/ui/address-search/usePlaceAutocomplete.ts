/**
 * usePlacesAutocomplete
 * -----------------------------------------------------------------------------
 * All the non-visual logic behind AddressSearch, built on Google Places API (New).
 *
 *   - Loads the Places library (cached forever by React Query).
 *   - Fetches suggestions for a debounced query.
 *   - Fetches full place details when a suggestion is selected.
 *
 * Billing note:
 *   Google groups the typing requests and the final place lookup into one
 *   billing "session" via an `AutocompleteSessionToken`. We start a new session
 *   when the library loads, and again after each successful selection or clear.
 *
 *   The session is tracked as `{ id, token }` and the numeric `id` goes in the
 *   query key. (The token object itself can't be used in a key: React Query
 *   serialises keys to JSON, so every token would look like `{}` and sessions
 *   would share cache entries.)
 */

import { keepPreviousData, useMutation, useQuery } from '@tanstack/react-query';
import { useState } from 'react';
import { loadPlacesLibrary } from '@/lib/googleMaps';
import type { Address } from '@/shared/types';
import { buildAddressFromPlace, PLACE_FIELDS } from './buildAddressFromPlace';
import { useDebouncedValue } from '@/shared/hooks/useDebouncedValue';

export interface SuggestionItem {
    key: string;
    label: string;
    /** Raw Google prediction, used later to fetch full place details. */
    prediction: google.maps.places.PlacePrediction;
}

interface Session {
    id: number;
    token: google.maps.places.AutocompleteSessionToken;
}

const createSession = (previous?: Session | null): Session => ({
    id: (previous?.id ?? 0) + 1,
    token: new google.maps.places.AutocompleteSessionToken(),
});

const MIN_QUERY_LENGTH = 2;

interface UsePlaceAutocompleteOptions {
    /** Text currently in the input (not debounced) */
    query: string;
    /** True when the text equals the already selected address, so we should not search again. */
    isSelectedText: boolean;
    /** List of region codes to restrict the search to. e.g. ['au'] */
    regionCodes: string[];
    debounceMs?: number;
}

export const usePlacesAutocomplete =({ query, isSelectedText, regionCodes, debounceMs = 300 }: UsePlaceAutocompleteOptions) => {
    const [session, setSession] = useState<Session | null>(null);

    // Load the Google Places library and create a new session if needed.
    const libraryQuery = useQuery({
        queryKey: ['google-places-library'],
        queryFn: async () => {
            await loadPlacesLibrary();
            return true;
        },
        staleTime: Infinity,
        retry: false,
    });
    const { isSuccess: isLibraryReady, error: libraryError } = libraryQuery;

    // If the library is ready but we don't have a session yet, create one.
    if (isLibraryReady && !session) {
        setSession(createSession());
    }

    // Suggestions
    const trimmedQuery = query.trim();
    // Debounce the query to avoid excessive API calls.
    const debouncedQuery = useDebouncedValue(trimmedQuery, debounceMs);

    const isSearchable =
        isLibraryReady &&
        !!session &&
        !isSelectedText &&
        trimmedQuery.length >= MIN_QUERY_LENGTH;  
    const isDebouncing = debouncedQuery !== trimmedQuery;
    const canFetch = isSearchable && !isDebouncing;

    /** Fetch address suggestions based on the debounced query. */
    const suggestionsQuery = useQuery({
        queryKey: ['places-autocomplete', session?.id, regionCodes.join(','), debouncedQuery],
        enabled: canFetch,
        staleTime: 60_000, // 60 seconds
        retry: false,
        placeholderData: keepPreviousData,
        queryFn: async (): Promise<SuggestionItem[]> => {
            const { suggestions } = await google.maps.places.AutocompleteSuggestion.fetchAutocompleteSuggestions({
                input: debouncedQuery,
                sessionToken: session!.token,
                includedRegionCodes: regionCodes,
            });

            return suggestions.flatMap((suggestion) => {
                const prediction = suggestion.placePrediction;
                const label = prediction?.text?.toString();
                return prediction && label 
                    ? [{ key: `${prediction.placeId}-${label}`, label, prediction }]
                    : [];
            });
        },
    });

    // Select a suggestion
    const selectMutation = useMutation({
        mutationFn: async (suggestion: SuggestionItem): Promise<Address> => {
            const place = suggestion.prediction.toPlace();
            await place.fetchFields({ fields: PLACE_FIELDS });
            
            const address = buildAddressFromPlace(place);
            if (!address) {
                throw new Error('Unable to map selected place to an address.');
            }
            return address;
        },
        onSuccess: () => setSession((previous) => createSession(previous)),
    });

    const resetSession = () => setSession((previous) => createSession(previous));

    let error: string | null = null;
    if (libraryError) {
        error = libraryError instanceof Error ? libraryError.message : 'Could not load Google Maps library.';
    } else if (selectMutation.isError) {
        error = 'Unable to fetch the selected address details.';
    } else if (suggestionsQuery.isError && isSearchable) {
        error = 'Unable to fetch address suggestions. Keep typing to try again.';
    }

    return {
        suggestions: isSearchable ? (suggestionsQuery.data ?? []) : [],
        isSearching: isSearchable && (isDebouncing || suggestionsQuery.isFetching),
        isSelecting: selectMutation.isPending,
        error,
        selectSuggestion: (suggestion: SuggestionItem, onSuccess: (address: Address) => void) => selectMutation.mutate(suggestion, { onSuccess }),
        clearSelectError: selectMutation.reset,
        resetSession,
    };
};
