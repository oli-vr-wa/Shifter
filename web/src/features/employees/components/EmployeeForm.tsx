import { useState } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import type { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { employeeService } from "../api/employee.service";
import { Button, FormField, FormSelect } from "@/components";
import { employeeRequestSchema, UserRole, type EmployeeRequest } from "../types";

interface EmployeeFormProps {
    initialData?: EmployeeRequest | null,
    onSuccess: () => void;
    onCancel: () => void;
}

type FormInput = z.input<typeof employeeRequestSchema>;
type FormOutput = z.output<typeof employeeRequestSchema>;

const roleOptions = [
    { label: "Company Owner", value: UserRole.CompanyOwner.toString() },
    { label: "Admin", value: UserRole.Admin.toString() },
    { label: "Employee", value: UserRole.Employee.toString() },
];

export const EmployeeForm = ({ initialData, onSuccess, onCancel }: EmployeeFormProps) => {
    const [error, setError] = useState<string | null>(null);
    const queryClient = useQueryClient();
    const isEditMode = !!initialData;

    const defaultFormData: EmployeeRequest = initialData || {
        firstName: '',
        lastName: '',
        email: '',
        phoneNumber: '',
        jobPosition: '',
        employmentStartDate: '',
        role: '' as unknown as UserRole,
    };
    
    const form = useForm<FormInput, any, FormOutput>({
        resolver: zodResolver(employeeRequestSchema),
        values: defaultFormData,
    });

    const { mutate: saveMutation, isPending } = useMutation({
        mutationFn: (data: EmployeeRequest) => 
            isEditMode && data.id
                ? employeeService.update(data.id, data)
                : employeeService.create(data)
        ,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["employees"] });
            onSuccess();
        },
        onError: (error: any) => {
            setError(error instanceof Error ? error.message : String(error));
        },
    });

    const handleSubmit = async (data: FormOutput) => {
        saveMutation(data);
    };

    return (
        <div className="grow w-full bg-white p-6 rounded-xl border border-gray-100 shadow-sm">
            <div className="border-b border-gray-100 pb-4 mb-6">
                <h2 className="text-2xl font-bold text-gray-900">
                    {isEditMode ? 'Edit Employee' : 'Add New Employee'}
                </h2>
                <p className="mt-1 text-gray-600 text-sm">
                    {isEditMode ? 'Update the details for this employee.' : 'Fill in the form below to add a new employee.'}
                </p>
            </div>
            
            <form onSubmit={form.handleSubmit(handleSubmit)} className="flex flex-col space-y-4">
                
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <FormField 
                        label="First Name"
                        type="text"
                        placeholder="Enter first name"
                        error={form.formState.errors.firstName?.message}
                        {...form.register('firstName')}
                    />
                    
                    <FormField
                        label="Last Name"
                        type="text"
                        placeholder="Enter last name"
                        error={form.formState.errors.lastName?.message}
                        {...form.register('lastName')}
                    />
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <FormField
                        label="Email"
                        type="email"
                        placeholder="email@example.com"
                        error={form.formState.errors.email?.message}
                        {...form.register('email')}
                    />

                    <FormField
                        label="Phone Number"
                        type="tel"
                        placeholder="0412 345 678"
                        error={form.formState.errors.phoneNumber?.message}
                        {...form.register('phoneNumber')}
                    />
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <FormField
                        label="Position"
                        type="text"
                        placeholder="Enter position"
                        error={form.formState.errors.jobPosition?.message}
                        {...form.register('jobPosition')}
                    />

                    <FormSelect
                        label="Role"
                        options={roleOptions}
                        placeholder="Select a role..."                        
                        error={form.formState.errors.role?.message}
                        {...form.register('role')}
                    />
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <FormField
                        label="Employment Start Date"
                        type="date"
                        placeholder="Select start date"
                        error={form.formState.errors.employmentStartDate?.message}
                        {...form.register('employmentStartDate')}
                    />
                </div>

                {/* Display any error messages from the error prop state */}
                {error && (
                    <div className="text-red-500">
                        {error}
                    </div>
                )}

                <div className="flex justify-end gap-3 pt-6 mt-6 border-t border-gray-100">
                    <Button
                        type="button" 
                        onClick={onCancel}
                        disabled={isPending}
                        theme="secondary"
                    >
                        Cancel
                    </Button>
                    <Button type="submit" disabled={isPending}>
                        {isPending ? 'Saving...' : (isEditMode ? 'Save Changes' : 'Add Employee')}
                    </Button>
                </div>
            </form>
        </div>
    );
};

