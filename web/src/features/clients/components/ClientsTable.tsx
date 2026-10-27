import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { useNavigate } from 'react-router-dom';
import { Badge } from '@/components/ui/Badge';
import { DataTable, type ColumnDef } from '@/components/ui/DataTable';
import { EnvelopeIcon, MapPinIcon, PencilSquareIcon, PhoneIcon } from '@heroicons/react/24/outline';
import { ClientType } from '@/features/clients/types';
import { clientService, type ClientListItem } from '@/features/clients/api/client.service';

export const ClientsTable = () => {
  const navigate = useNavigate();
  const [searchTerm, setSearchTerm] = useState('');

  const { data: clientsData, isLoading, isError } = useQuery({
    queryKey: ['clients', searchTerm],
    queryFn: async () =>
      (await clientService.getAll({
        searchTerm: searchTerm.trim() || undefined,
        clientType: undefined,
        pageNumber: 1,
        pageSize: 20,
      })).data,
  });

  const handleNavigateToEditClient = (clientId: string) => {
    navigate(`/clients/${clientId}/edit`);
  };

  const columns: ColumnDef<ClientListItem>[] = [
    {
      header: 'CLIENT',
      cell: (row) => (
        <span className="font-medium text-gray-900">{row.displayName}</span>
      ),
    },
    {
      header: 'TYPE',
      cell: (row) => (
        <Badge type={row.type === ClientType.Company ? 'info' : 'success'}>
          {row.type === ClientType.Company ? 'Company' : 'Individual'}
        </Badge>
      ),
    },
    {
      header: 'CONTACT',
      cell: (row) => (
        <div className="flex flex-col gap-1 text-sm text-gray-600">
          {row.email ? (
            <div className="flex items-center gap-2"><EnvelopeIcon className="w-4 h-4" /> {row.email}</div>
          ) : null}
          {row.phoneNumber ? (
            <div className="flex items-center gap-2"><PhoneIcon className="w-4 h-4" /> {row.phoneNumber}</div>
          ) : null}
          {!row.email && !row.phoneNumber ? <span>No contact details</span> : null}
        </div>
      ),
    },
    {
      header: 'ADDRESS',
      cell: (row) => (
        <div className="flex items-center gap-2 text-sm text-gray-600">
          <MapPinIcon className="w-4 h-4 shrink-0" />
          <span>{row.address || 'No address provided'}</span>
        </div>
      ),
    },
    {
      header: 'ACTIONS',
      className: 'text-right',
      cell: (row) => (
        <div className="flex items-center justify-end gap-3">
          <button
            onClick={() => handleNavigateToEditClient(row.id)}
            className="text-gray-400 hover:text-teal-600 cursor-pointer"
            aria-label={`Edit ${row.displayName}`}
          >
            <PencilSquareIcon className="w-5 h-5" />
          </button>
        </div>
      ),
    },
  ];

  return (
    <div className="space-y-4">
      <DataTable
        columns={columns}
        data={clientsData?.items ?? []}
        isLoading={isLoading}
        isLoadError={isError}
        showToolbar={true}
        searchTerm={searchTerm}
        onSearchChange={setSearchTerm}
        searchPlaceholder="Search client..."
        showFilters={true}
      />
    </div>
  );
};
