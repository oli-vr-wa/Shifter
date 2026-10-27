import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Button, FormField } from "@/components";
import { clientContactSchema, type ClientContact } from "../types";

interface ClientContactFormProps {
    onAdd: (contact: ClientContact) => void;
    onCancel: () => void;
}

export const ClientContactForm = ({ onAdd, onCancel }: ClientContactFormProps) => {
    const { register, handleSubmit, reset, formState: { errors } } = useForm<ClientContact>({
        resolver: zodResolver(clientContactSchema),
        defaultValues: { firstName: "", lastName: "", jobTitle: "", email: "", phoneNumber: "" },
    });

    const submit = handleSubmit((contact) => {
        onAdd(contact);
        reset();
    });

    return (
        <div className="mb-4 rounded-lg border border-gray-200 bg-white p-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">

                <FormField label="First Name" error={errors.firstName?.message} {...register("firstName")} />
                <FormField label="Last Name" error={errors.lastName?.message} {...register("lastName")} />
                <FormField label="Job Title" error={errors.jobTitle?.message} {...register("jobTitle")} />
                <FormField label="Email" type="email" error={errors.email?.message} {...register("email")} />
                <FormField label="Phone Number" type="tel" error={errors.phoneNumber?.message} {...register("phoneNumber")} />

            </div>
            <div className="mt-4 flex justify-end gap-3">
                <Button type="button" theme="secondary" onClick={onCancel}>Cancel</Button>
                <Button type="button" onClick={submit}>Save Contact</Button>
            </div>
        </div>
  );
};