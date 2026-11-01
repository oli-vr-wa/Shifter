import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { useState } from 'react';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import '@testing-library/jest-dom/vitest';
import type { Address } from '@/shared/types';
import { usePlacesAutocomplete, type SuggestionItem } from './usePlacesAutocomplete';
import AddressSearch from './AddressSearch';

vi.mock('./usePlacesAutocomplete');
const mockedHook = vi.mocked(usePlacesAutocomplete);

const makeSuggestion = (label: string): SuggestionItem => ({
    key: label,
    label,
    prediction: {} as SuggestionItem['prediction'],
});

const suggestions = [makeSuggestion('12 Hay St, Perth WA 6000'), makeSuggestion('10 James St, Northbridge WA 6003')];

const makeAddress = (displayName: string): Address => ({ placeId: 'p1', displayName }) as Address;

const baseHookResult = {
    suggestions: [] as SuggestionItem[],
    isSearching: false,
    isSelecting: false,
    error: null as string | null,
    selectSuggestion: vi.fn(),
    clearSelectError: vi.fn(),
    resetSession: vi.fn(),
}

const mockHook = (overrides: Partial<typeof baseHookResult> = {}) =>
    mockedHook.mockReturnValue({ ...baseHookResult, ...overrides });

beforeEach(() => {
  vi.clearAllMocks();
  mockHook();
});

const Controlled = ({
    initial = null,
    onChangeSpy,
} : { 
    initial?: Address | null; 
    onChangeSpy: (address: Address | null) => void 
}) => {
    const [value, setValue] = useState<Address | null>(initial);
    return (
        <AddressSearch
            value={value}
            onChange={(address) => {
                onChangeSpy(address);
                setValue(address);
            }}
        />
    );            
};

// --- Tests for AddressSearch component ---

describe('AddressSearch', () => {
    it('links the label to the input', () => {
        render(<AddressSearch label="Home Address" />);
        expect(screen.getByLabelText('Home Address')).toBeInTheDocument();
    });

    it('shows suggestions when the input is focused', async () => {
        mockHook({ suggestions });
        const user = userEvent.setup();
        render(<AddressSearch />);

        expect(screen.queryByRole('option')).not.toBeInTheDocument();

        await user.click(screen.getByRole('combobox'));

        expect(screen.getAllByRole('option')).toHaveLength(suggestions.length);
        expect(screen.getByRole('option', { name: suggestions[0].label })).toBeInTheDocument();
        expect(screen.getByRole('option', { name: suggestions[1].label })).toBeInTheDocument();
    });

    it('selects a suggestion on click and reports the address', async () => {
        const address = makeAddress(suggestions[0].label);
        const selectSuggestion = vi.fn((_suggestion, onSuccess) => onSuccess(address));        
        mockHook({ suggestions, selectSuggestion });
        const onChangeSpy = vi.fn();
        const user = userEvent.setup();
        render(<Controlled onChangeSpy={onChangeSpy} />);

        await user.click(screen.getByRole('combobox'));
        await user.click(screen.getByRole('option', { name: suggestions[0].label }));
        expect(screen.getByRole('combobox')).toHaveValue(address.displayName);
    });

    describe('keyboard', () => {
        it('ArrowDown highlights options and Enter selects the highlighted one', async () => {
            const selectSuggestion = vi.fn();
            mockHook({ suggestions, selectSuggestion });
            const user = userEvent.setup();
            render(<Controlled onChangeSpy={vi.fn()} />);

            await user.click(screen.getByRole('combobox'));
            await user.keyboard('{ArrowDown}{ArrowDown}');

            expect(screen.getByRole('option', { name: suggestions[1].label })).toHaveAttribute('aria-selected', 'true');

            await user.keyboard('{Enter}');
            expect(selectSuggestion).toHaveBeenCalledWith(suggestions[1], expect.any(Function));
        });

        it('ArrowUp from the start wraps to the last option', async () => {
            mockHook({ suggestions });
            const user = userEvent.setup();
            render(<AddressSearch />);

            await user.click(screen.getByRole('combobox'));
            await user.keyboard('{ArrowUp}');

            expect(screen.getByRole('option', { name: suggestions[suggestions.length - 1].label })).toHaveAttribute('aria-selected', 'true');
        }); 

        it('Enter does nothing when no option is highlighted', async () => {
            const selectSuggestion = vi.fn();
            mockHook({ suggestions, selectSuggestion });
            const user = userEvent.setup();
            render(<AddressSearch />);

            await user.click(screen.getByRole('combobox'));
            await user.keyboard('{Enter}');

            expect(selectSuggestion).not.toHaveBeenCalled();
        });

        it('Escape closes the suggestions list', async () => {
            mockHook({ suggestions });
            const user = userEvent.setup();
            render(<AddressSearch />);

            await user.click(screen.getByRole('combobox'));
            expect(screen.getAllByRole('option')).toHaveLength(suggestions.length);

            await user.keyboard('{Escape}');
            expect(screen.queryByRole('option')).not.toBeInTheDocument();
        });        
    });

    it('closes the suggestions list when focus leaves the component', async () => {
        mockHook({ suggestions });
        const user = userEvent.setup();
        render(
            <>
                <AddressSearch />
                <button type="button">Outside</button>
            </>
        );

        await user.click(screen.getByRole('combobox'));
        expect(screen.getAllByRole('option')).toHaveLength(suggestions.length);

        await user.click(screen.getByRole('button', { name: 'Outside' }));;
        expect(screen.queryByRole('option')).not.toBeInTheDocument();
    });

    // Testing bug: Editing a selected address, used to wipe the whole input after the first keystroke.
    it('keeps the text and clears the selection when a selected address is edited', async() => {
        const onChangeSpy = vi.fn();
        const user = userEvent.setup();
        render(<Controlled initial={makeAddress('1 Foo St')} onChangeSpy={onChangeSpy} />);

        const input = screen.getByRole('combobox');
        expect(input).toHaveValue('1 Foo St');

        await user.type(input, 'x');

        expect(onChangeSpy).toHaveBeenCalledWith(null);
        expect(input).toHaveValue('1 Foo Stx');
    });

    it('clears the input and the selection when the clear button is clicked', async () => {
        const onChangeSpy = vi.fn();
        const user = userEvent.setup();
        render(<Controlled initial={makeAddress('1 Foo St')} onChangeSpy={onChangeSpy} />);

        await user.click(screen.getByRole('button', { name: 'Clear' }));

        expect(screen.getByRole('combobox')).toHaveValue('');
        expect(onChangeSpy).toHaveBeenCalledWith(null);
        expect(baseHookResult.resetSession).toHaveBeenCalled();
    });

    it('does not show the clear button when the input is empty', async () => {
        render(<AddressSearch />);
        expect(screen.queryByRole('button', { name: 'Clear' })).not.toBeInTheDocument();
    });

    it('shows both the validation error and the loading error', () => {
        mockHook({ error: 'Unable to load address suggestions.'});
        render(<AddressSearch error="Address is required" />);

        expect(screen.getByText('Address is required')).toBeInTheDocument();
        expect(screen.getByText('Unable to load address suggestions.')).toBeInTheDocument();
    });
});