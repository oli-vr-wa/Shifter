import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { Button, FormField, FormSelect } from '@/components';
import { ClientType, clientRequestSchema, type ClientRequest } from '@/features/clients/types';
import { ClientContactsSection } from './ClientContactsSection';
import { useSaveClient } from '../hooks/useSaveClient';


interface ClientFormProps {
  initialData?: ClientRequest | null;
  onSuccess: () => void;
  onCancel: () => void;
}

const clientTypeOptions = [
  { label: 'Individual', value: String(ClientType.Individual) },
  { label: 'Company', value: String(ClientType.Company) },
];

const defaultClientData: ClientRequest = {
  type: ClientType.Individual,
  companyName: '',
  abn: '',
  firstName: '',
  lastName: '',
  email: '',
  phoneNumber: '',
  address: null,
  contacts: [],
};

export const ClientForm = ({ initialData, onSuccess, onCancel }: ClientFormProps) => {
  const isEditMode = !!initialData;    
  const form = useForm<ClientRequest>({
    resolver: zodResolver(clientRequestSchema as any),
    defaultValues: initialData || defaultClientData,
  });
  const errors = form.formState.errors as Record<string, any>;


  const selectedType = Number(form.watch('type') ?? ClientType.Individual);

  const { mutate, isPending, error } = useSaveClient(isEditMode, onSuccess);   

  return (
    <div className="grow w-full bg-white p-6 rounded-xl border border-gray-100 shadow-sm">
      <div className="border-b border-gray-100 pb-4 mb-6">
        <h2 className="text-2xl font-bold text-gray-900">
          {isEditMode ? 'Edit Client' : 'Add New Client'}
        </h2>
        <p className="mt-1 text-gray-600 text-sm">
          {isEditMode ? 'Update the details for this client.' : 'Fill in the form below to add a new client.'}
        </p>
      </div>

      <form onSubmit={form.handleSubmit((d) => mutate(d))} className="flex flex-col space-y-4">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <FormSelect
            label="Client Type"
            options={clientTypeOptions}
            placeholder="Select client type"
            error={errors.type?.message}
            {...form.register('type')}
          />
        </div>

        {selectedType === ClientType.Individual ? (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FormField
              label="First Name"
              type="text"
              placeholder="Enter first name"
              error={errors.firstName?.message}
              {...form.register('firstName')}
            />

            <FormField
              label="Last Name"
              type="text"
              placeholder="Enter last name"
              error={errors.lastName?.message}
              {...form.register('lastName')}
            />
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FormField
              label="Company Name"
              type="text"
              placeholder="Enter company name"
              error={errors.companyName?.message}
              {...form.register('companyName')}
            />

            <FormField
              label="ABN"
              type="text"
              placeholder="Enter ABN"
              error={errors.abn?.message}
              {...form.register('abn')}
            />
          </div>
        )}

        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <FormField
            label="Email"
            type="email"
            placeholder="email@example.com"
            error={errors.email?.message}
            {...form.register('email')}
          />

          <FormField
            label="Phone Number"
            type="tel"
            placeholder="0412 345 678"
            error={errors.phoneNumber?.message}
            {...form.register('phoneNumber')}
          />
        </div>

        {selectedType === ClientType.Company && (
          <ClientContactsSection control={form.control} error={errors.contacts?.root?.message ?? errors.contacts?.message} />
        )}

        {error && <div className="text-red-500">{error.message}</div>}

        <div className="flex justify-end gap-3 pt-6 mt-6 border-t border-gray-100">
          <Button type="button" onClick={onCancel} disabled={isPending} theme="secondary">
            Cancel
          </Button>
          <Button type="submit" disabled={isPending}>
            {isPending ? 'Saving...' : isEditMode ? 'Save Changes' : 'Add Client'}
          </Button>
        </div>
      </form>
    </div>
  );
};
