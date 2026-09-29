import { Routes, Route, Navigate } from "react-router-dom";
import Login from "@/pages/Login";
import ConfirmEmail from "@/pages/ConfirmEmail";
import Dashboard from "@/pages/Dashboard";
import ProtectedRoute from "@/features/auth/components/ProtectedRoute";
import GuestRoute from "@/features/auth/components/GuestRoute";

export default function App() {
    return (
        <Routes>
            <Route element={<GuestRoute />}>
                <Route path="/" element={<Navigate to="/login" />} />
                <Route path="/login" element={<Login />} />
                <Route path="/confirm-email" element={<ConfirmEmail />} />
            </Route>

            <Route element={<ProtectedRoute />}>
                <Route path="/dashboard" element={<Dashboard />} />
            </Route>
        </Routes>
    );
}