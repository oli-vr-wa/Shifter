/// <reference types="google.maps" />

/**
 * AddressSearch
 * -----------------------------------------------------------------------------
 * A controlled address autocomplete input backed by Google Places API (New).
 * All Google/React Query logic lives in `usePlacesAutocomplete`; this file is
 * only state for the text box, the dropdown and keyboard handling.
 *
 * Usage:
 *   <AddressSearch
 *     value={address}            // currently selected Address (or null)
 *     onChange={setAddress}      // called with Address on select, null on clear/edit
 *     error={errors.address}     // optional validation message
 *     regionCodes={['au']}       // optional, defaults to Australia
 *   />
 *
 * Resetting from outside:
 *   Changing `value` to another address updates the text. Setting it to `null`
 *   does NOT clear the text (that is also what happens when the user edits a
 *   selected address). To fully reset, remount: `<AddressSearch key={formKey} />`.
 */

import { useState, useId, useRef, type KeyboardEvent } from 'react';
import type { Address } from '@/shared/types';
import Input from '../../Input';
import { usePlacesAutocomplete, type SuggestionItem } from './usePlaceAutocomplete';

interface AddressSearchProps {
  value?: Address | null;
  onChange?: (address: Address | null) => void;
  label?: string;
  placeholder?: string;
  error?: string;
  disabled?: boolean;
  regionCodes?: string[];
}

const DEFAULT_REGION_CODES = ['au'];


export const AddressSearch = ({
  value,
  onChange,
  label = 'Address',
  placeholder = 'Search for a street address',
  error,
  disabled = false,
  regionCodes = DEFAULT_REGION_CODES
}: AddressSearchProps) => {
  const inputId = useId();
  const listboxId = `${inputId}-listbox`;
  const statusId = `${inputId}-status`;

  /** Text currently in the input field */
  const [query, setQuery] = useState(value?.displayName ?? '');
  const [isOpen, setIsOpen] = useState(false);
  const [activeIndex, setActiveIndex] = useState(-1);

  const editVersion = useRef(0);

  const [syncedName, setSyncedName] = useState(value?.displayName);
  if (value?.displayName !== syncedName) {
    setSyncedName(value?.displayName);
    if (value) {
      setQuery(value.displayName);
    }
  }

  const isSelectedText = !!value && query.trim() === value.displayName.trim();

  const {
    suggestions,
    isSearching,
    isSelecting,
    error: loadingError,
    selectSuggestion,
    clearSelectError,
    resetSession, 
  } = usePlacesAutocomplete({ query, isSelectedText, regionCodes });

  const showList = isOpen && !disabled && suggestions.length > 0;
  const isBusy = isSearching || isSelecting;
  

  // --- Handlers --------------------------------------------------------------

  const handleInputChange = (nextQuery: string) => {
    editVersion.current += 1;
    clearSelectError();
    setQuery(nextQuery);
    setIsOpen(true);
    setActiveIndex(-1);
    if (value && nextQuery !== value.displayName) {
      onChange?.(null);
    }
  }

  const handleSelect = (suggestion: SuggestionItem) => {
    const version = editVersion.current;
    setIsOpen(false);
    setActiveIndex(-1);

    selectSuggestion(suggestion, (address) => {
      if (version !== editVersion.current) return; // The user edited or cleared while the query was being fetched

      setQuery(address.displayName);
      onChange?.(address);
    });
  };

  /** Clears the input, the dropdown and the selection, and starts a new session. */
  const handleClear = () => {
    editVersion.current += 1;
    clearSelectError();
    resetSession();
    setQuery('');
    setIsOpen(false);
    setActiveIndex(-1);
    onChange?.(null);
  };

  const handleKeyDown = (event: KeyboardEvent<HTMLInputElement>) => {
    switch (event.key) {
      case 'ArrowDown':
        if (suggestions.length === 0) return;
        event.preventDefault();
        setIsOpen(true);
        setActiveIndex((index) => (index + 1) % suggestions.length);
        break;
      case 'ArrowUp':
        if (suggestions.length === 0) return;
        event.preventDefault();
        setIsOpen(true);
        setActiveIndex((index) => (index <= 0 ? suggestions.length - 1 : index - 1));
        break;
      case 'Enter':
        if (showList && activeIndex >= 0) {
          event.preventDefault();
          handleSelect(suggestions[activeIndex]);
        }
        break;
      case 'Escape': {
        if (showList) {
          event.preventDefault();
          setIsOpen(false);
          setActiveIndex(-1);
        }
        break;
      }
      default:
        break;
    }
  };
    

  return (
    <div
      className="flex flex-col relative"
      // Close the dropdown when focus leaves the whole component.
      onBlur={(event) => {
        if (!event.currentTarget.contains(event.relatedTarget)) {
          setIsOpen(false);
          setActiveIndex(-1);
        }
      }}
    >
      {label && (
        <label htmlFor={inputId} className="mb-1 text-sm font-medium text-gray-700">
          {label}
        </label>
      )}
 
      <div className="relative">
        <Input
          id={inputId}
          name="address-search-query"
          value={query}
          onChange={(event) => handleInputChange(event.target.value)}
          onKeyDown={handleKeyDown}
          onFocus={() => setIsOpen(true)}
          placeholder={placeholder}
          disabled={disabled}
          className="w-full"
          autoComplete="off"
          data-lpignore="true"
          data-1p-ignore
          role="combobox"
          aria-autocomplete="list"
          aria-expanded={showList}
          aria-controls={listboxId}
          aria-activedescendant={showList && activeIndex >= 0 ? `${inputId}-option-${activeIndex}` : undefined}
          aria-describedby={statusId}
          aria-invalid={!!error}
          aria-busy={isBusy}
        />
 
        {isBusy && (
          <svg
            className="absolute right-14 top-1/2 h-4 w-4 -translate-y-1/2 animate-spin text-gray-400"
            viewBox="0 0 24 24"
            fill="none"
            aria-hidden="true"
          >
            <circle cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" className="opacity-25 text-teal-500" />
            <path d="M4 12a8 8 0 018-8" stroke="currentColor" strokeWidth="4" strokeLinecap="round" className="text-teal-500" />
          </svg>
        )}
 
        {query && !disabled && (
          <button
            type="button"
            onClick={handleClear}
            className="absolute right-2 top-1/2 -translate-y-1/2 text-xs text-gray-500 hover:text-gray-800"
          >
            Clear
          </button>
        )}
 
        {/* Always rendered so aria-controls never points at a missing element. */}
        <ul
          id={listboxId}
          role="listbox"
          aria-label={`${label} suggestions`}
          hidden={!showList}
          // Keep focus in the input when the list is clicked.
          onMouseDown={(event) => event.preventDefault()}
          className="absolute left-0 right-0 top-full z-20 mt-1 max-h-64 overflow-y-auto rounded-md border border-gray-200 bg-white shadow-lg"
        >
          {showList &&
            suggestions.map((suggestion, index) => {
              const isActive = index === activeIndex;
              return (
                <li
                  key={suggestion.key}
                  id={`${inputId}-option-${index}`}
                  role="option"
                  aria-selected={isActive}
                  ref={isActive ? (element) => element?.scrollIntoView({ block: 'nearest' }) : undefined}
                  onClick={() => handleSelect(suggestion)}
                  onMouseEnter={() => setActiveIndex(index)}
                  className={`w-full cursor-pointer border-b border-gray-100 px-3 py-2 text-left text-sm last:border-b-0 ${
                    isActive ? 'bg-gray-100' : 'hover:bg-gray-50'
                  }`}
                >
                  {suggestion.label}
                </li>
              );
            })}
        </ul>
      </div>
 
      {/* Screen-reader announcement of how many results are available. */}
      <span id={statusId} role="status" aria-live="polite" className="sr-only">
        {showList ? `${suggestions.length} suggestions available. Use up and down arrows to review.` : ''}
      </span>
 
      {error && <span className="mt-1 text-sm text-red-500">{error}</span>}
      {loadingError && <span className="mt-1 text-sm text-red-500">{loadingError}</span>}
    </div>
  );
};
 
export default AddressSearch;
