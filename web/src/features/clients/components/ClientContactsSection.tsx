import type { ClientContact, ClientRequest } from "../types";
import { useFieldArray, type Control } from "react-hook-form";
import { useState } from "react";
import { ClientContactForm } from "./ClientContactForm";
import { DataTable, type ColumnDef } from "@/components/ui/DataTable"
import { Button } from "@/components";


export const ClientContactsSection = ({ control, error }: { control: Control<ClientRequest>; error?: string }) => {
    const [isAdding, setIsAdding] = useState(false);
    const { fields, append, remove } = useFieldArray({
        control,
        name: "contacts"
    });

    const columns: ColumnDef<(typeof fields)[number]>[] = [
        { header: "Name", cell: (c) => `${c.firstName} ${c.lastName}` },
        { header: "Job Title", cell: (c) => c.jobTitle },
        { header: "Email", cell: (c) => c.email },
        { header: "Phone", cell: (c) => c.phoneNumber },
        {
            header: "Actions",
            className: "text-right",
            cell: (c) => (
                <button type="button" className="text-red-600 hover:text-red-700"
                        onClick={() => remove(fields.findIndex((f) => f.id === c.id))}>
                        Remove
                </button>
            ),
        },
    ];

    const handleAdd = (contact: ClientContact) => {
        append(contact);
        setIsAdding(false);
    };

    return (
        <div className="rounded-xl border border-gray-200 bg-gray-50 p-4">
            <div className="mb-4 flex items-center justify-between gap-3">
                <div>
                    <h3 className="text-lg font-semibold text-gray-900">Client Contacts</h3>
                    <p className="text-sm text-gray-600">Add one or more company contacts.</p>
                </div>
                {!isAdding && (
                    <Button type="button" theme="secondary" onClick={() => setIsAdding(true)}>
                        Add Contact
                    </Button>
                )}
            </div>

            {isAdding && <ClientContactForm onAdd={handleAdd} onCancel={() => setIsAdding(false)} />}

            <DataTable
                columns={columns}
                data={fields}
                getRowKey={(c) => c.id}
                emptyMessage="No contacts added yet."
                compact
            />
        </div>
    );
};