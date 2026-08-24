import { Link } from "@/components";

export const RegistrationCompleted = () => {
    return (
        <div className="grow w-full max-w-xl">
            <h2 className="mb-3 text-left text-2xl font-bold text-gray-900">Registration Completed</h2>
            <p className="mb-10 text-left text-gray-600 text-sm">Thank you for registering! Please check your email to confirm your account.</p>

            <hr className="my-5 border-gray-300" />
            <p className="text-center text-gray-600 text-sm">Already confirmed your email? <Link href="/login">Login</Link></p>
        </div>
    );
}