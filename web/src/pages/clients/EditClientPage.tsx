import { useNavigate, useParams } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { ClientForm } from '@/features/clients';
import { clientService } from '@/features/clients/api/client.service';
import type { ClientRequest } from '@/features/clients/types';
import { ClientType } from '@/features/clients/types';

export default function EditClientPage() {
  const { clientId } = useParams<{ clientId: string }>();
  const navigate = useNavigate();

  const { data: clientData, isLoading, isError } = useQuery({
    queryKey: ['client', clientId],
    queryFn: async () => (await clientService.getById(clientId!)).data,
    enabled: !!clientId,
  });

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isError || !clientData) {
    return <div>Error loading client data.</div>;
  }

  const initialData: ClientRequest = {
    id: clientData.id ?? '',
    type: clientData.type ?? ClientType.Individual,
    companyName: clientData.companyName ?? '',
    abn: clientData.abn ?? '',
    firstName: clientData.firstName ?? '',
    lastName: clientData.lastName ?? '',
    email: clientData.email ?? '',
    phoneNumber: clientData.phoneNumber ?? '',
    address: clientData.address ?? null,
    contacts: clientData.contacts ?? [],
  };

  return (
    <div className="w-full max-w">
      <ClientForm
        initialData={initialData}
        onSuccess={() => navigate('/clients')}
        onCancel={() => navigate('/clients')}
      />
    </div>
  );
}
