import { useNavigate } from 'react-router-dom';
import { PlusIcon } from '@heroicons/react/24/outline';
import Button from '@/components/Button';
import { ClientsTable } from '@/features/clients';

export default function ClientsPage() {
  const navigate = useNavigate();

  return (
    <div className="w-full max-w">
      <div className="flex justify-between items-start mb-8">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 mb-1">Clients</h1>
          <p className="text-gray-500 text-sm">Manage clients, contact details, and active job relationships.</p>
        </div>

        <Button
          className="flex items-center gap-2"
          onClick={() => navigate('/clients/new')}
        >
          <PlusIcon className="w-5 h-5" />
          Add Client
        </Button>
      </div>

      <ClientsTable />
    </div>
  );
}
