import apiClient from "@/lib/client";
import type { Employee } from "@/types/models/employee";
import type { EmployeeRequest } from "../types";

export const employeeService = {
    getCurrent: () => apiClient.get<Employee[]>("/employees/current"),
    create: (data: EmployeeRequest) => apiClient.post("/employees", data),
    update: (id: string, data: EmployeeRequest) => apiClient.put(`/employees/${id}`, data),                
};