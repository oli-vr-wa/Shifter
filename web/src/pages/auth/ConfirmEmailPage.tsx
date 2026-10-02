import { useEffect, useRef, useState } from "react";
import { useSearchParams } from "react-router";
import { authService } from "@/features/auth";
import { Link } from "@/components";
import { StandardPageTemplate } from "@/components/StandardPageTemplate";

export default function ConfirmEmailPage() {
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
        <StandardPageTemplate>
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
        </StandardPageTemplate>
    );
}