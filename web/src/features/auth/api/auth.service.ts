import { apiClient } from "@/lib/client";
import { type LoginRequest, type LoginResponse, type RegisterRequest } from "@/features/auth/types";

export const authService = {
    login: (data: LoginRequest) => apiClient.post<LoginResponse>("/auth/login", data),
    register: (data: RegisterRequest) => apiClient.post("/auth/register", data),
};