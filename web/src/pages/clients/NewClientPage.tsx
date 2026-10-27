import { useNavigate } from 'react-router-dom';
import { ClientForm } from '@/features/clients';

export default function NewClientPage() {
  const navigate = useNavigate();

  return (
    <div className="w-full max-w">
      <ClientForm
        onSuccess={() => navigate('/clients')}
        onCancel={() => navigate('/clients')}
      />
    </div>
  );
}
