import { useState } from "react";
import { useMutation } from "@tanstack/react-query";
import { authService } from "../api/auth.service";
import type { LoginRequest } from "../types";

interface LoginFormProps {
    onSuccess: () => void;
}

export const LoginForm = ({ onSuccess }: LoginFormProps) => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const {mutate: loginMutation, isPending} = useMutation({
        mutationFn: (data: LoginRequest) => authService.login(data),
        onSuccess: () => onSuccess(),
    });

    const handleSubmit = (e: React.SubmitEvent) => {
        e.preventDefault();
        loginMutation({ email, password });
    };

    return (
        <form onSubmit={handleSubmit} className="flex flex-col space-y-4">

            <div className="flex flex-col">
                <label htmlFor="email" className="mb-1 text-sm font-medium text-gray-700">Email:</label>
                <input
                    type="email"
                    id="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                    className="rounded-md border border-gray-300 p-2 focus:border-blue-500 focus:outline-hidden focus:ring-1 focus:ring-blue-500"
                />
            </div>
            <div className="flex flex-col">
                <label htmlFor="password" className="mb-1 text-sm font-medium text-gray-700">Password:</label>
                <input
                    type="password"
                    id="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                    className="rounded-md border border-gray-300 p-2 focus:border-blue-500 focus:outline-hidden focus:ring-1 focus:ring-blue-500"
                />
            </div>
            <button 
                type="submit"
                className="mt-2 rounded-md bg-blue-600 px-4 py-2 font-medium text-white hover:bg-blue-700 disabled:cursor-not-allowed disabled:bg-blue-300"
                disabled={isPending}
                >
                    {isPending ? 'Logging in...' : 'Login'}
            </button>
        </form>
    );
};