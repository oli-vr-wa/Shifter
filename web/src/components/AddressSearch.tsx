/// <reference types="google.maps" />

/**
 * AddressSearch
 * -----------------------------------------------------------------------------
 * A controlled address autocomplete input backed by Google Places API (New).
 *
 * How it works (high level):
 *   1. On mount, the Google Places library is loaded (see `@/lib/googleMaps`).
 *   2. As the user types, suggestions are fetched (debounced) with
 *      `AutocompleteSuggestion.fetchAutocompleteSuggestions`.
 *   3. When the user picks a suggestion, the full place details are fetched
 *      with `Place.fetchFields` and converted into our own `Address` shape.
 *   4. The parent is notified through `onChange`.
 *
 * Billing note:
 *   Google groups the typing requests and the final place lookup into one
 *   billing "session" via an `AutocompleteSessionToken`. We create a new token
 *   on load and again after each selection or clear, so each search is one
 *   session.
 *
 * Usage:
 *   <AddressSearch
 *     value={address}            // currently selected Address (or null)
 *     onChange={setAddress}      // called with Address on select, null on clear/edit
 *     error={errors.address}     // optional validation message
 *   />
 */

import { useEffect, useState } from 'react';
import { loadPlacesLibrary } from '@/lib/googleMaps';
import type { Address } from '@/shared/types';

interface AddressSearchProps {
  value?: Address | null;
  onChange?: (address: Address | null) => void;
  label?: string;
  placeholder?: string;
  error?: string;
  disabled?: boolean;
}

interface SuggestionItem {
  key: string;
  label: string;
  prediction: google.maps.places.PlacePrediction; // The raw Google Places prediction object for this suggestion to be used when fetching full place details
}

/**
 * Finds the first address component of a given type (e.g. "postal_code") and
 * returns its long text. Returns an empty string if the component is missing.
 */
const getComponentValue = (components: google.maps.places.AddressComponent[] | undefined, type: string) =>
  components?.find((component) => component.types.includes(type))?.longText ?? '';

/**
 * Converts a Google `Place` into our app's `Address` type.
 *
 * The place must have had its fields fetched first (see `handlePlaceSelect`),
 * otherwise `addressComponents`, `location`, etc. will be undefined.
 *
 * @returns the mapped Address, or `null` if the place has no id.
 */
const buildAddressFromPlace = (place: google.maps.places.Place): Address | null => {
  if (!place.id) {
    return null;
  }
  const latitude = place.location?.lat() ?? null;
  const longitude = place.location?.lng() ?? null;
  const displayName = place.formattedAddress ?? place.displayName ?? '';

  return {
    placeId: place.id,
    displayName,
    streetNumber: getComponentValue(place.addressComponents, 'street_number'),
    streetName: getComponentValue(place.addressComponents, 'route'),
    suburb: getComponentValue(place.addressComponents, 'locality')
      || getComponentValue(place.addressComponents, 'sublocality')
      || getComponentValue(place.addressComponents, 'postal_town')
      || getComponentValue(place.addressComponents, 'administrative_area_level_2')
      || '',
    postCode: getComponentValue(place.addressComponents, 'postal_code'),
    state: getComponentValue(place.addressComponents, 'administrative_area_level_1'),
    country: getComponentValue(place.addressComponents, 'country'),
    latitude: latitude ?? undefined,
    longitude: longitude ?? undefined,
  };
};

// -----------------------------------------------------------------------------
// Component
// -----------------------------------------------------------------------------

export const AddressSearch = ({
  value,
  onChange,
  label = 'Address',
  placeholder = 'Search for a street address',
  error,
  disabled = false,
}: AddressSearchProps) => {
  /** Text currently in the input field */
  const [query, setQuery] = useState(value?.displayName ?? '');
  /** Suggestions returned from the Google Places API based on the current query */
  const [suggestions, setSuggestions] = useState<SuggestionItem[]>([]);
  const [loading, setLoading] = useState(false);
  const [apiReady, setApiReady] = useState(false);
  const [sessionToken, setSessionToken] = useState<google.maps.places.AutocompleteSessionToken | null>(null);
  const [loadingError, setLoadingError] = useState<string | null>(null);

  // --- Effects ---------------------------------------------------------------

  /**
   * Effect 1: keep the input in sync with the `value` prop.
   *
   * If the parent changes the selected address (e.g. loading a saved record or
   * resetting the form), the text box follows. Without this, `query` would only
   * be initialised once on first render.
   */
  useEffect(() => {
    setQuery(value?.displayName ?? '');
  }, [value?.displayName]);

  /**
   * Effect 2: load the Google Places library once, on mount.
   *
   * When loaded, marks the API as ready and creates the first session token.
   * The `cancelled` flag prevents state updates after unmount (and during
   * React StrictMode's double-mount in development).
   */
  useEffect(() => {
    let cancelled = false;

    loadPlacesLibrary()
      .then(() => {
        if (cancelled) {
          return;
        }

        setApiReady(true);
        setSessionToken(new google.maps.places.AutocompleteSessionToken());
        setLoadingError(null);
      })
      .catch((error: unknown) => {
        if (!cancelled) {
          setLoadingError(error instanceof Error ? error.message : 'Could not load Google Maps.');
        }
      });

    return () => {
      cancelled = true;
    };
  }, []);

  /**
   * Effect 3: fetch suggestions as the user types (debounced by 300 ms).
   *
   * Skips the request (and clears the dropdown) when:
   *   - the Places library isn't ready yet,
   *   - the query is shorter than 2 characters, or
   *   - the query equals the already-selected address (so the dropdown doesn't
   *     reopen right after the user picks something).
   *
   * Two guards protect against stale results:
   *   - `clearTimeout(timer)` cancels a pending request when the user keeps typing.
   *   - `active` is set to false in the cleanup, so a slow response from an older
   *     query can never overwrite the results of a newer one.
   */
  useEffect(() => {
    const normalizedQuery = query.trim();
    const normalizedSelectedAddress = value?.displayName?.trim() ?? '';

    if (!apiReady || normalizedQuery.length < 2 || (normalizedSelectedAddress && normalizedQuery === normalizedSelectedAddress)) {
      setSuggestions([]);
      setLoading(false);
      return;
    }

    let active = true;
    setLoading(true);
    setLoadingError(null);

    const timer = setTimeout(() => {
      google.maps.places.AutocompleteSuggestion.fetchAutocompleteSuggestions({
        input: query,
        sessionToken: sessionToken ?? undefined,
        includedRegionCodes: ['au'],
      })
        .then(({ suggestions: nextSuggestions }) => {
          if (!active) {
            return;
          }

          setLoading(false);
          const mappedSuggestions = nextSuggestions
            .map((suggestion): SuggestionItem | null => {
              const prediction = suggestion.placePrediction;

              if (!prediction) {
                return null;
              }

              const label = prediction.text?.toString() ?? '';
              if (!label) {
                return null;
              }

              return {
                key: `${prediction.placeId}-${label}`,
                label,
                prediction,
              };
            })
            .filter((suggestion): suggestion is SuggestionItem => suggestion !== null);

          setSuggestions(mappedSuggestions);
        })
        .catch(() => {
          if (!active) {
            return;
          }

          setLoading(false);
          setSuggestions([]);
        });
    }, 300); // Debounce for 300ms

    return () => {
      clearTimeout(timer);
      active = false;      
    };
  }, [apiReady, query, sessionToken, value?.displayName]);

  // --- Handlers --------------------------------------------------------------

  /**
   * Runs when the user clicks a suggestion.
   *
   * Converts the prediction into a Place, fetches only the fields we need
   * (requesting fewer fields keeps billing lower), maps it to our `Address`
   * type, and notifies the parent. A fresh session token is created afterwards
   * because the previous session ended with this lookup.
   */
  const handlePlaceSelect = async (selectedSuggestion: SuggestionItem) => {
    setLoading(true);
    setLoadingError(null);
    setSuggestions([]);

    try {
      const selectedPlace = selectedSuggestion.prediction.toPlace();
      await selectedPlace.fetchFields({
        fields: ['id', 'formattedAddress', 'displayName', 'addressComponents', 'location'],
      });

      const selectedAddress = buildAddressFromPlace(selectedPlace);
      if (!selectedAddress) {
        throw new Error('Unable to map selected place to an address.');
      }

      setSuggestions([]);
      setQuery(selectedAddress.displayName);
      onChange?.(selectedAddress);
      setSessionToken(new google.maps.places.AutocompleteSessionToken());
    } catch {
      setLoadingError('Unable to fetch the selected address details.');
    } finally {
      setLoading(false);
    }
  };

  /** Clears the input, the dropdown and the selection, and starts a new session. */
  const handleClear = () => {
    setQuery('');
    setSuggestions([]);
    setLoadingError(null);
    setSessionToken(new google.maps.places.AutocompleteSessionToken());
    onChange?.(null);
  };

  return (
    <div className="flex flex-col relative">
      {label && (
        <label className="mb-1 text-sm font-medium text-gray-700">{label}</label>
      )}

      <div className="relative">
        <input
          value={query}
          onChange={(event) => {
            const nextQuery = event.target.value;
            setQuery(nextQuery);
            if (value && nextQuery !== value.displayName) {
              onChange?.(null);
            }
          }}
          placeholder={placeholder}
          disabled={disabled}
          className="w-full rounded-md border border-gray-300 p-2 focus:border-teal-500 focus:outline-hidden focus:ring-1 focus:ring-teal-500 disabled:bg-gray-100 disabled:text-gray-500 disabled:cursor-not-allowed aria-invalid:border-red-500 aria-invalid:focus:border-red-500 aria-invalid:focus:ring-red-500"
          aria-invalid={!!error}
        />

        {query && !disabled && (
          <button
            type="button"
            onClick={handleClear}
            className="absolute right-2 top-1/2 -translate-y-1/2 text-xs text-gray-500 hover:text-gray-800"
          >
            Clear
          </button>
        )}
        {suggestions.length > 0 && !disabled && (
          <ul className="absolute left-0 right-0 top-full z-20 mt-1 max-h-64 overflow-y-auto rounded-md border border-gray-200 bg-white shadow-lg">
            {suggestions.map((suggestion) => (
              <li key={suggestion.key}>
                <button
                  type="button"
                  onClick={() => void handlePlaceSelect(suggestion)}
                  className="w-full cursor-pointer border-b border-gray-100 px-3 py-2 text-left text-sm hover:bg-gray-50 last:border-b-0"
                >
                  {suggestion.label}
                </button>
              </li>
            ))}
          </ul>
        )}
      </div>

      {loading && (
        <span className="mt-1 text-xs text-gray-500">Loading address suggestions...</span>
      )}

      {error && <span className="mt-1 text-sm text-red-500">{error}</span>}
      {loadingError && <span className="mt-1 text-sm text-red-500">{loadingError}</span>}
    </div>
  );
};

export default AddressSearch;
