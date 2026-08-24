import apiClient from "@/lib/client";
import { type LoginRequest, type LoginResponse, type RegisterRequest } from "@/features/auth/types";

export const authService = {
    login: async (data: LoginRequest) => { return (await apiClient.post<LoginResponse>("/auth/login", data)).data; },
    register: async (data: RegisterRequest) => await apiClient.post("/auth/register", data),
    confirmEmail: async (userId: string, token: string) => await apiClient.get("/auth/verify-email", { params: { userId, token } }),
    verifyTwoFactorAuth: async (data: { code: string, mfa: string }) => await apiClient.post("/auth/verify-2fa", { code: data.code }, { headers: { Authorization: `Bearer ${data.mfa}` } }),
    me: async () => await apiClient.get("/auth/me"),
    logout: async () => await apiClient.post("/auth/logout"),
};