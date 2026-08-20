import { LoginForm } from '@/features/auth';
import { useNavigate } from 'react-router-dom';

export default function Login() {
    const navigate = useNavigate();

    const handleLoginSuccess = () => {
        navigate("/dashboard");
    };

    return (
        <div className="flex min-h-screen items-center justify-center bg-gray-100 p-4">
            <div className="w-full max-w-md rounded-lg bg-white p-8 shadow-md">
                <h2 className="mb-6 text-center text-2xl font-bold text-gray-900">Sign In to Shifter</h2>
                <LoginForm onSuccess={handleLoginSuccess} />
            </div>
        </div>
    );
}