import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation } from "@tanstack/react-query";
import { authService } from "../api/auth.service";
import { type LoginRequest, loginRequestSchema } from "../types";
import type { LoginResponse } from "../types";
import { FormField, Button, Link } from "@/components"; 
import type { AxiosError } from 'axios';

interface LoginFormProps {
    onSuccess: (response: LoginResponse) => void;
    onClickForgotPassword?: () => void;
    onClickRegister?: () => void;
}

export const LoginForm = ({ onSuccess, onClickForgotPassword, onClickRegister }: LoginFormProps) => {

    const defaultFormData: LoginRequest = {
        email: '',
        password: '',
    };

    const form = useForm<LoginRequest>({
        resolver: zodResolver(loginRequestSchema),
        defaultValues: defaultFormData,
    });

    const { mutate: loginMutation, isPending } = useMutation({
        mutationFn: (data: LoginRequest) => authService.login(data),
        onSuccess: (response: LoginResponse) => onSuccess(response),
        onError: (error: AxiosError<{message?: string } | string>) => {
            const errorMessage = typeof error.response?.data === 'string'
                ? error.response.data
                : error.response?.data?.message || 'Invalid email or password.';
            form.setError('root', { type: 'server', message: errorMessage }); 
        }
    });

    const handleSubmit = async (data: LoginRequest) => {
        loginMutation(data);
    }

    return (
        <div className="grow w-full max-w-xl flex flex-col justify-center">
            <h2 className="mb-3 text-left text-2xl font-bold text-gray-900">Welcome back</h2>
            <p className="mb-10 text-left text-gray-600 text-sm">Please enter your credentials to access your account.</p>
            <form onSubmit={form.handleSubmit(handleSubmit)} className="flex flex-col space-y-4">
        
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

                <div className="flex justify-end">
                    <Link href="#" onClick={onClickForgotPassword}>Forgot password?</Link>
                </div>

                <Button type="submit" disabled={isPending} >
                    {isPending ? 'Logging in...' : 'Login'}
                </Button>

            </form>
            
            <hr className="my-10 border-gray-300" />
            <p className="text-center text-gray-600 text-sm">Don't have an account? <Link href="#" onClick={onClickRegister}>Start your free trial.</Link></p>
        </div>
    );
};