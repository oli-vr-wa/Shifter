import { useMutation } from "@tanstack/react-query";
import { authService } from "../api/auth.service";
import { type RegisterRequest, registerRequestSchema } from "../types";
import { Button, Link, FormField } from "@/components";
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from "react-hook-form";
import { z } from "zod";

interface RegisterFormProps {
    onSuccess: () => void;
    onClickLogin?: () => void;
}

type FormInput = z.input<typeof registerRequestSchema>;
type FormOutput = z.output<typeof registerRequestSchema>;

export const RegisterForm = ({ onSuccess, onClickLogin }: RegisterFormProps) => {
    const defaultFormData: RegisterRequest = {
        firstName: '',
        lastName: '',
        email: '',
        password: '',
        confirmPassword: '',
        companyName: '',
        companyAbn: '',
    };

    const form = useForm<FormInput, any, FormOutput>({
        resolver: zodResolver(registerRequestSchema),
        defaultValues: defaultFormData,
    });

    const { mutate: registerMutation, isPending } = useMutation({
        mutationFn: (data: RegisterRequest) => authService.register(data),
        onSuccess: () => onSuccess(),
    });

    const handleSubmit = async (data: FormOutput) => {
        registerMutation(data);
    }

    return (
        <div className="grow w-full max-w-xl">
            <h2 className="mb-3 text-left text-2xl font-bold text-gray-900">Create an account</h2>
            <p className="mb-10 text-left text-gray-600 text-sm">Please fill in the form below to create your account.</p>
            <form onSubmit={form.handleSubmit(handleSubmit)} className="flex flex-col space-y-4">

                <FormField 
                    label="First Name"
                    type="text"
                    placeholder="Enter your first name"
                    error={form.formState.errors.firstName?.message}
                    {...form.register('firstName')}
                />
                
                <FormField
                    label="Last Name"
                    type="text"
                    placeholder="Enter your last name"
                    error={form.formState.errors.lastName?.message}
                    {...form.register('lastName')}
                />

                <FormField
                    label="Email"
                    type="email"
                    placeholder="email@example.com"
                    error={form.formState.errors.email?.message}
                    {...form.register('email')}
                />

                <FormField
                    label="Password"
                    type="password"
                    placeholder="Enter your password"
                    error={form.formState.errors.password?.message}
                    {...form.register('password')}
                />  

                <FormField
                    label="Confirm Password"
                    type="password"
                    placeholder="Confirm your password"
                    error={form.formState.errors.confirmPassword?.message}
                    {...form.register('confirmPassword')}
                />  

                <FormField
                    label="Company Name"
                    type="text"
                    placeholder="Enter your company name"
                    error={form.formState.errors.companyName?.message}
                    {...form.register('companyName')}
                />  

                <FormField
                    label="Company ABN"
                    type="text"
                    placeholder="Enter your company ABN"
                    error={form.formState.errors.companyAbn?.message}
                    {...form.register('companyAbn')}
                />
                                
                <Button type="submit" disabled={isPending}>
                    {isPending ? 'Registering...' : 'Register'}
                </Button>

            </form>
            <hr className="my-5 border-gray-300" />
            <p className="text-center text-gray-600 text-sm">Already have an account? <Link href="#" onClick={onClickLogin}>Login</Link></p>
        </div>
    );
}
