import { render, screen, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { createElement } from 'react';
import { describe, expect, it, vi } from 'vitest';
import { DataTable, type ColumnDef, type DataTableProps } from './DataTable';

// --- Fictures & Helpers

interface Person {
    id: string;
    name: string;
    age: number | null;
}

const people: Person[] = [
    { id: '1', name: 'Alice', age: 30 },
    { id: '2', name: 'Bob', age: null },
    { id: '3', name: 'Charlie', age: 25 },
];

const columns: ColumnDef<Person>[] = [
    { header: 'Name', accessorKey: 'name' },
    { header: 'Age', accessorKey: 'age' }
];

const renderTable = (props: Partial<DataTableProps<Person>> = {}) =>
    render(createElement(DataTable<Person>, { columns, data: people, ...props }));

const getBodyRows = () => screen.getAllByRole('row').slice(1);

// --- Tests

describe('DataTable', () => {
    describe('headers', () => {
        it('renders a column header for each column', () => {
            renderTable();

            expect(screen.getByRole('columnheader', { name: 'Name' })).toBeInTheDocument();
            expect(screen.getByRole('columnheader', { name: 'Age' })).toBeInTheDocument();
        });

        it('forwards the column className to the header and body cells', () => {
            renderTable({
                columns: [{ header: 'Actions', accessorKey: 'name', className: 'text-right' }],
            });
 
            expect(screen.getByRole('columnheader', { name: 'Actions' })).toHaveClass('text-right');
            expect(screen.getByRole('cell', { name: 'Alice' })).toHaveClass('text-right');
        });
    });

    describe('rows and cells', () => {
        it('renders one body row per data item', () => {
            renderTable();
 
            expect(getBodyRows()).toHaveLength(3);
        });
 
        it('renders values using accessorKey', () => {
            renderTable();
 
            expect(screen.getByRole('cell', { name: 'Alice' })).toBeInTheDocument();
            expect(screen.getByRole('cell', { name: '30' })).toBeInTheDocument();
        });
 
        it('renders an empty cell when the accessorKey value is null or undefined', () => {
            renderTable();
 
            const bobCells = within(getBodyRows()[1]).getAllByRole('cell');
 
            expect(bobCells[0]).toHaveTextContent('Bob');
            expect(bobCells[1].textContent).toBe('');
        });
 
        it('renders an empty cell when a column has neither cell nor accessorKey', () => {
            renderTable({ columns: [{ header: 'Nothing' }] });
 
            const cells = within(getBodyRows()[0]).getAllByRole('cell');
 
            expect(cells).toHaveLength(1);
            expect(cells[0].textContent).toBe('');
        });
 
        it('uses the custom cell renderer when provided', () => {
            renderTable({
                columns: [
                    {
                        header: 'Name',
                        cell: (row) => createElement('strong', { 'data-testid': 'custom' }, row.name.toUpperCase()),
                    },
                ],
            });
 
            const custom = screen.getAllByTestId('custom');
            expect(custom).toHaveLength(3);
            expect(custom[0]).toHaveTextContent('ALICE');
        });
 
        it('prefers the cell renderer over accessorKey when both are set', () => {
            renderTable({
                columns: [{ header: 'Name', accessorKey: 'name', cell: () => 'from renderer' }],
            });
 
            expect(screen.getAllByText('from renderer')).toHaveLength(3);
            expect(screen.queryByText('Alice')).not.toBeInTheDocument();
        });
 
        it('calls getRowKey with each row and its index', () => {
            const getRowKey = vi.fn((row: Person) => row.id);
 
            renderTable({ getRowKey });
 
            expect(getRowKey).toHaveBeenCalledWith(people[0], 0);
            expect(getRowKey).toHaveBeenCalledWith(people[1], 1);
            expect(getRowKey).toHaveBeenCalledWith(people[2], 2);
        });
 
        it('renders no body rows for empty data', () => {
            renderTable({ data: [] });
 
            expect(screen.queryAllByRole('row')).toHaveLength(1); // header row only
        });
    });
 
    describe('toolbar', () => {
        it('is hidden by default', () => {
            renderTable();
 
            expect(screen.queryByRole('textbox')).not.toBeInTheDocument();
            expect(screen.queryByRole('button', { name: /filters/i })).not.toBeInTheDocument();
        });
 
        it('shows the search input when showToolbar is true', () => {
            renderTable({ showToolbar: true });
 
            expect(screen.getByRole('textbox')).toBeInTheDocument();
        });
 
        it('uses the default placeholder', () => {
            renderTable({ showToolbar: true });
 
            expect(screen.getByPlaceholderText('Search...')).toBeInTheDocument();
        });
 
        it('uses a custom placeholder', () => {
            renderTable({ showToolbar: true, searchPlaceholder: 'Search client...' });
 
            expect(screen.getByPlaceholderText('Search client...')).toBeInTheDocument();
        });
 
        it('reflects the searchTerm prop as the input value', () => {
            renderTable({ showToolbar: true, searchTerm: 'alice' });
 
            expect(screen.getByRole('textbox')).toHaveValue('alice');
        });
 
        it('calls onSearchChange with the typed value', async () => {
            const user = userEvent.setup();
            const onSearchChange = vi.fn();
            renderTable({ showToolbar: true, onSearchChange });
 
            await user.type(screen.getByRole('textbox'), 'a');
 
            expect(onSearchChange).toHaveBeenCalledWith('a');
        });
 
        it('does not throw when typing without an onSearchChange handler', async () => {
            const user = userEvent.setup();
            renderTable({ showToolbar: true });
 
            await expect(user.type(screen.getByRole('textbox'), 'abc')).resolves.not.toThrow();
        });
    });
 
    describe('filters button', () => {
        it('is hidden when showFilters is false', () => {
            renderTable({ showToolbar: true, showFilters: false });
 
            expect(screen.queryByRole('button', { name: /filters/i })).not.toBeInTheDocument();
        });
 
        it('is hidden when showFilters is true but the toolbar is not shown', () => {
            renderTable({ showToolbar: false, showFilters: true });
 
            expect(screen.queryByRole('button', { name: /filters/i })).not.toBeInTheDocument();
        });
 
        it('is shown when both showToolbar and showFilters are true', () => {
            renderTable({ showToolbar: true, showFilters: true });
 
            expect(screen.getByRole('button', { name: /filters/i })).toBeInTheDocument();
        });
 
        it('calls onFilterClick when clicked', async () => {
            const user = userEvent.setup();
            const onFilterClick = vi.fn();
            renderTable({ showToolbar: true, showFilters: true, onFilterClick });
 
            await user.click(screen.getByRole('button', { name: /filters/i }));
 
            expect(onFilterClick).toHaveBeenCalledTimes(1);
        });
    });
 
    describe('loading, error and empty states', () => {
        it('shows a loading message when isLoading is true', () => {
            renderTable({ isLoading: true });
 
            expect(screen.getByText('Loading...')).toBeInTheDocument();
        });
 
        it('shows an error message when isLoadError is true', () => {
            renderTable({ isLoadError: true });
 
            expect(screen.getByText('Error loading data.')).toBeInTheDocument();
        });
 
        it('shows neither loading nor error by default', () => {
            renderTable();
 
            expect(screen.queryByText('Loading...')).not.toBeInTheDocument();
            expect(screen.queryByText('Error loading data.')).not.toBeInTheDocument();
        });
 
        it('keeps rendering existing rows while loading', () => {
            renderTable({ isLoading: true });
 
            expect(getBodyRows()).toHaveLength(3);
        });
 
        it('shows the empty message when there is no data', () => {
            renderTable({ data: [], emptyMessage: 'No people found' });
 
            expect(screen.getByText('No people found')).toBeInTheDocument();
        });
 
        it('does not show an empty message when emptyMessage is not provided', () => {
            renderTable({ data: [] });
 
            expect(screen.queryByText(/no .* found/i)).not.toBeInTheDocument();
        });
 
        it('does not show the empty message when there is data', () => {
            renderTable({ emptyMessage: 'No people found' });
 
            expect(screen.queryByText('No people found')).not.toBeInTheDocument();
        });
 
        it('does not show the empty message while loading', () => {
            renderTable({ data: [], isLoading: true, emptyMessage: 'No people found' });
 
            expect(screen.getByText('Loading...')).toBeInTheDocument();
            expect(screen.queryByText('No people found')).not.toBeInTheDocument();
        });
 
        it('does not show the empty message when there is a load error', () => {
            renderTable({ data: [], isLoadError: true, emptyMessage: 'No people found' });
 
            expect(screen.getByText('Error loading data.')).toBeInTheDocument();
            expect(screen.queryByText('No people found')).not.toBeInTheDocument();
        });
    });
});

