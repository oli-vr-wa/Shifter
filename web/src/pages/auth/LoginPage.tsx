import { useNavigate } from 'react-router-dom';
import shifterLogo from '@/assets/shifter-logo-v1.svg';
import shifterLoginImage from '@/assets/login-page-image.png';
import { useState } from 'react';
import { LoginForm, RegisterForm, RegistrationCompleted, TwoFactorAuthForm } from '@/features/auth';
import type { LoginResponse } from '@/features/auth';

export default function LoginPage() {
    const [currentView, setCurrentView] = useState<'login' | 'twoFactorAuth' | 'forgotPassword' | 'register' | 'registrationCompleted'>('login');
    const [loginResponse, setLoginResponse] = useState<LoginResponse | null>(null);
    const navigate = useNavigate();

    const handleLoginSuccess = (response : LoginResponse) => {
        setLoginResponse(response);
        setCurrentView('twoFactorAuth');        
    };

    const handleRegisterSuccess = () => {
        setCurrentView('registrationCompleted');
    }

    const handleForgotPasswordClick = () => {
        setCurrentView('forgotPassword');
    };

    const handleRegisterClick = () => {
        setCurrentView('register');
    };

    return (
        <div className="flex flex-row min-h-screen bg-white">

            <div className="hidden relative md:flex w-full flex-col justify-center overflow-hidden bg-teal-600 p-12 text-white rounded-2xl m-3">

                <div className="absolute inset-0 z-0">
                    <div className="absolute -top-24 right-0 h-[150%] w-1/2 -skew-x-12 translate-x-1/3 bg-teal-700/50"></div>
                    <div className="absolute -bottom-24 left-0 h-[150%] w-1/3 -skew-x-12 -translate-x-1/2 bg-teal-500/20"></div>
                </div>

                <div className="relative z-10 flex flex-col gap-15">
                    <div>
                        <h1 className="mb-6 text-4xl font-bold">Operations, simplified.</h1>
                        <p className="mb-6 text-lg">
                            Shifter takes the friction out of team management. Coordinate schedules, assign tasks seamlessly, and keep everyone aligned in real-time.
                        </p>
                    </div>                    
                    <img src={shifterLoginImage} alt="Shifter Login" className="w-full max-w-lg" />
                </div>
            </div>

            <div className="flex flex-col min-h-screen w-full bg-white p-12 pb-7 items-center">
                <div className="w-full left-6 top-6 sm:left-12 sm:top-12 mb-5">
                    <a href="#" className="flex items-center gap-2">
                        <img src={shifterLogo} alt="Shifter Logo" className="h-8 w-auto" />
                    </a>
                </div>                                  
                {currentView === 'login' ? (
                    <LoginForm onSuccess={handleLoginSuccess} onClickForgotPassword={handleForgotPasswordClick} onClickRegister={handleRegisterClick} />
                ) : currentView === 'register' ? (
                    <RegisterForm onSuccess={handleRegisterSuccess} onClickLogin={() => setCurrentView('login')} />
                ) : currentView === 'registrationCompleted' ? (
                    <RegistrationCompleted />
                ) : currentView === 'twoFactorAuth' && loginResponse ? (
                    <TwoFactorAuthForm onSuccess={() => navigate('/dashboard')} loginResponse={loginResponse} />
                ) : (
                    <div className="text-center text-gray-600 text-sm">Forgot Password functionality is not implemented yet.</div>
                )}

                <div className="w-full bottom-6 left-6 sm:left-12 text-gray-400 text-sm mt-5">
                    &copy; {new Date().getFullYear()} Shifter Pty Ltd. All rights reserved.
                </div>
            </div>
            
        </div>
    );
}