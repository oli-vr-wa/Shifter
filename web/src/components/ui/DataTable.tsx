import { MagnifyingGlassIcon, FunnelIcon } from "@heroicons/react/24/outline";
import type { ReactNode } from "react";

export interface ColumnDef<T> {
    header: string;
    accessorKey?: keyof T;
    cell?: (row: T) => ReactNode;
    className?: string;
}

export interface DataTableProps<T> {
    columns: ColumnDef<T>[];
    data: T[];
    isLoading?: boolean;
    isLoadError?: boolean;
    // Optional toolbar props
    showToolbar?: boolean;
    searchTerm?: string;
    onSearchChange?: (value: string) => void;
    searchPlaceholder?: string;
    showFilters?: boolean;
    onFilterClick?: () => void;
}

export function DataTable<T>({ 
    columns, 
    data,
    showToolbar = false,
    isLoading = false,
    isLoadError = false,
    searchTerm = '',
    onSearchChange,
    searchPlaceholder = "Search...",
    showFilters = false,
    onFilterClick
}: DataTableProps<T>) {
    return (
        <div className="bg-white rounded-xl border border-gray-100 shadow-sm overflow-hidden">

            {/* Conditionally render the toolbar only if requested */}
            {showToolbar && (
                <div className="p-4 border-b border-gray-100 flex justify-between items-center bg-white">
                <div className="relative w-72">
                    <MagnifyingGlassIcon className="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                    <input
                    type="text"
                    placeholder={searchPlaceholder}
                    className="w-full pl-10 pr-4 py-2 rounded-lg border border-gray-200 text-sm focus:outline-none focus:ring-2 focus:ring-[#0d3330] focus:border-transparent"
                    value={searchTerm}
                    onChange={(e) => onSearchChange?.(e.target.value)}
                    />
                </div>
                
                {showFilters && (
                    <button 
                    onClick={onFilterClick}
                    className="flex items-center gap-2 text-gray-600 hover:text-gray-900 px-3 py-2 rounded-lg border border-gray-200 text-sm font-medium"
                    >
                    <FunnelIcon className="w-4 h-4" />
                    Filters
                    </button>
                )}
                </div>
            )}
            
            <div className="overflow-x-auto">                
                <table className="w-full text-left border-collapse">
                    <thead>
                        <tr className="bg-gray-50 border-b border-gray-100">
                            {columns.map((col, index) => (
                                <th
                                    key={index} 
                                    className={`px-6 py-4 text-xs font-medium text-gray-500 uppercase tracking-wider ${col.className || ''}`}
                                >
                                    {col.header}
                                </th>
                            ))}
                        </tr>
                    </thead>
                    <tbody className="divide-y divide-gray-100">
                        {data.map((row, rowIndex) => (
                            <tr key={rowIndex} className="hover:bg-gray-50 transition-colors">
                            {columns.map((col, colIndex) => (
                                <td 
                                    key={colIndex} 
                                    className={`px-6 py-4 ${col.className || ''}`}
                                    >
                                    {/* If a custom cell renderer is provided, use it. Otherwise, print the raw data. */}
                                    {col.cell 
                                        ? col.cell(row) 
                                        : (col.accessorKey ? String(row[col.accessorKey]) : null)
                                    }
                                </td>
                            ))}
                        </tr>
                    ))}
                    </tbody>
                </table>
                {isLoading && (
                    <div className="p-4 text-center text-gray-500">
                        Loading...
                    </div>
                )}
                {isLoadError && (
                    <div className="p-4 text-center text-red-500">
                        Error loading data.
                    </div>
                )}
            </div>
        </div>
    )
}