import { useState } from "react";
import type { LoginResponse } from "../types";
import { QRCode } from "react-qr-code";
import { Button, FormField } from "@/components";
import { TwoFactorAuthenticationRequestSchema, type TwoFactorAuthenticationRequest } from "../types/two-factor-authentication-request.types";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { authService } from "../api/auth.service";
import { useMutation } from "@tanstack/react-query";
import { useAuth } from "../hooks/AuthContext";

interface TwoFactorAuthFormProps {
    onSuccess: () => void;
    loginResponse: LoginResponse;
}

export const TwoFactorAuthForm = ({ onSuccess, loginResponse }: TwoFactorAuthFormProps) => {
    const [error, setError] = useState<string | null>(null);
    const { setAuthUser } = useAuth();

    const defaultFormData: TwoFactorAuthenticationRequest = {
        code: '',
    };

    const form = useForm<TwoFactorAuthenticationRequest>({
        resolver: zodResolver(TwoFactorAuthenticationRequestSchema),
        defaultValues: defaultFormData,
    });

    const { mutate: verifyTwoFactorAuthMutation, isPending } = useMutation({
        mutationFn: (data: TwoFactorAuthenticationRequest) => authService.verifyTwoFactorAuth({ ...data, mfa: loginResponse.mfaToken }),
        onSuccess: (response) => {      
            console.log(response);      
            setAuthUser(response.data.user);
            onSuccess();
        },
        onError: (error: any) => {
            setError(error instanceof Error ? error.message : String(error));
        }
    });

    const handleSubmit = async (data: TwoFactorAuthenticationRequest) => {
        verifyTwoFactorAuthMutation(data);
    }

    return (
        <div className="grow w-full max-w-xl flex flex-col justify-center">
            <h2 className="mb-3 text-left text-2xl font-bold text-gray-900">Two-Factor Authentication</h2>
            {loginResponse.setupRequired ? (
                <>
                    <p className="mb-10 text-left text-gray-600 text-sm">To enhance the security of your account, please set up two-factor authentication (2FA) using an authenticator app.</p>
                    <div className="mb-5">
                        <p className="text-left text-gray-600 text-sm">Scan the QR code below with your authenticator app or use the manual entry key provided.</p>
                        {loginResponse.qrCodeUri && (
                            <div className="my-4 bg-white p-4 rounded-md justify-center items-center flex">
                                <QRCode value={loginResponse.qrCodeUri} className="h-auto max-w-2xs w-full" size={256} viewBox="0 0 256 256" />
                            </div>
                        )}
                        {loginResponse.manualEntryKey && (
                            <div className="my-4 text-center">
                                <p className="text-gray-600 text-sm">Manual Entry Key: <span className="font-mono">{loginResponse.manualEntryKey}</span></p>
                            </div>
                        )}
                    </div>
                </>
            ) : null}
            
            <p className="mb-10 text-left text-gray-600 text-sm">Please enter the 6-digit code from your authenticator app to complete the login process.</p>
            <form className="flex flex-col space-y-4" onSubmit={form.handleSubmit(handleSubmit)}>
                
                <FormField 
                    label="Authentication Code"
                    type="text"
                    placeholder="Enter 6-digit code"
                    error={form.formState.errors.code?.message}
                    {...form.register('code')}
                />

                <Button type="submit">
                    {isPending ? 'Verifying...' : 'Verify Code'}
                </Button>
                {error && <p className="text-red-500 text-sm mt-2">{error}</p>} 
            </form>
        </div>
    );
}