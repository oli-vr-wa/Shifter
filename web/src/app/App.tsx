import { Routes, Route, Navigate } from "react-router-dom";
import LoginPage from "@/pages/auth/LoginPage";
import ConfirmEmailPage from "@/pages/auth/ConfirmEmailPage";
import DashboardPage from "@/pages/dashboard/DashboardPage";
import EmployeesPage from "@/pages/employees/EmployeesPage";
import { ProtectedRoute, GuestRoute } from "@/features/auth";
import { AppLayout } from "@/components/layouts/AppLayout";
import NewEmployeePage from "@/pages/employees/NewEmployeePage";
import EditEmployeePage from "@/pages/employees/EditEmployeePage";
import ClientsPage from "@/pages/clients/ClientsPage";
import NewClientPage from "@/pages/clients/NewClientPage";
import EditClientPage from "@/pages/clients/EditClientPage";

export default function App() {
    return (
        <Routes>
            <Route element={<GuestRoute />}>
                <Route path="/" element={<Navigate to="/login" />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/confirm-email" element={<ConfirmEmailPage />} />
            </Route>

            <Route element={<ProtectedRoute />}>
                <Route element={<AppLayout />}>
                    <Route path="/dashboard" element={<DashboardPage />} />
                    <Route path="/employees" element={<EmployeesPage />} />
                    <Route path="/employees/new" element={<NewEmployeePage />} />
                    <Route path="/employees/:employeeId/edit" element={<EditEmployeePage />} />
                    <Route path="/clients" element={<ClientsPage />} />
                    <Route path="/clients/new" element={<NewClientPage />} />
                    <Route path="/clients/:clientId/edit" element={<EditClientPage />} />
                </Route>
            </Route>
        </Routes>
    );
}