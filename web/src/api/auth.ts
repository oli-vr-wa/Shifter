import { apiClient } from "./client";
import { type LoginRequest, type LoginResponse, type RegisterRequest } from "../types/auth";

export const loginUser = async (data: LoginRequest): Promise<LoginResponse> => {
    const response = await apiClient.post<LoginResponse>("/auth/login", data);
    return response.data;
};

export const registerUser = async (data: RegisterRequest): Promise<void> => {
    await apiClient.post("/auth/register", data);
};