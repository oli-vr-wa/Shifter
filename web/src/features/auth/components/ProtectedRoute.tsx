import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../hooks/AuthContext";

export const ProtectedRoute = () => {
    const { isAuthenticated, isLoading } = useAuth();

    if (isLoading) {

        return (
            <div className="flex items-center justify-center h-screen">
                <div className="w-16 h-16 border-4 border-dashed rounded-full animate-spin border-teal-600"></div>
            </div>
        );
    }

    return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
}