import React, { useEffect, useRef, useState } from "react";
import shifterLogo from '@/assets/shifter-logo-v1.svg';
import { useSearchParams } from "react-router";
import { authService } from "@/features/auth";
import { Link } from "@/components";

export default function ConfirmEmail() {
    const [searchParams] = useSearchParams();
    const [status, setStatus] = useState<'loading' | 'success' | 'error'>('loading');
    const [error, setError] = useState<string | null>(null);
    const hasFetched = useRef(false);

    useEffect(() => {
        const confirmEmail = async () => {
            if (hasFetched.current) return;
            const userId = searchParams.get('userId');
            const token = searchParams.get('token');

            if (!userId || !token) {
                setStatus('error');
                return;
            }

            hasFetched.current = true;

            try {
                const response = await authService.confirmEmail(userId, token);

                if (response.status === 200) {
                    setStatus('success');
                } else {
                    setStatus('error');
                    setError('Failed to confirm email');
                }
            } catch (error) {
                setStatus('error');
                setError(error instanceof Error ? error.message : String(error));
            }
        };

        confirmEmail();
    }, [searchParams]);

    const titleClass = "mb-3 text-left text-2xl font-bold text-gray-900";
    const messageClass = "mb-10 text-left text-gray-600 text-sm";    

    return (
        <div className="relative flex flex-col min-h-screen items-center justify-center bg-teal-400">

            <div className="absolute inset-0 z-0 overflow-hidden">
                <div className="absolute -top-24 right-0 h-[150%] w-1/2 -skew-x-12 translate-x-1/3 bg-teal-700/50"></div>
                <div className="absolute -bottom-24 left-0 h-[150%] w-1/3 -skew-x-12 -translate-x-1/2 bg-teal-500/20"></div>
            </div>

            <div className="relative z-10 flex flex-col items-center justify-center w-full max-w-xl h-full bg-white p-8 rounded-2xl -mt-40 m-3 shadow-2xl">

                <a href="#" className="justify-center mb-12">
                    <img src={shifterLogo} alt="Shifter Logo" className="h-8 w-auto" />
                </a>

                {status === 'loading' && (
                    <>
                        <h2 className={titleClass}>Confirming your email...</h2>
                        <p className={messageClass}>Please wait while we confirm your email address.</p>
                    </>
                )}

                {status === 'success' && (
                    <>
                        <h2 className={titleClass}>Email confirmed!</h2>
                        <p className={messageClass}>Your email address has been successfully confirmed. You can now log in to your account.</p>
                        <Link href="/Login">Go to Login</Link>
                    </>
                )}

                {status === 'error' && (
                    <>
                        <h2 className={titleClass}>Error confirming email</h2>
                        <p className={messageClass}>There was an error confirming your email address. Please try again or contact support.</p>
                        {error && <p className="text-red-500 text-sm">{error}</p>}
                    </>
                )}
            </div>
        </div>
    );
}