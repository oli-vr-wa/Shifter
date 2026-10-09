import { z } from "zod";

export const UserRole = {
    CompanyOwner: 0,
    Admin: 1,
    Employee: 2
} as const;

export type UserRole = (typeof UserRole)[keyof typeof UserRole];

const userRoleValues = [UserRole.CompanyOwner, UserRole.Admin, UserRole.Employee] as const;

export const employeeRequestSchema = z.object({
    id: z.uuid().optional(),
    firstName: z.string().min(1, "First name is required"),
    lastName: z.string().min(1, "Last name is required"),
    email: z.email("Invalid email address").min(1, "Email is required"),
    phoneNumber: z.string().min(1, "Phone number is required"),
    jobPosition: z.string().min(1, "Position is required"),
    employmentStartDate: z.string().min(1, "Employment start date is required"),
    role: z.preprocess(
        // If it's an empty string or undefined, pass undefined to fail the required check. Otherwise, cast to Number.
        (val) => (val === "" || val === undefined ? undefined : Number(val)),
        z.number({
            message: "Please select a valid role"
        }).refine(
            (value): value is UserRole => userRoleValues.includes(value as UserRole),
            { message: "Please select a valid role" }
        )
    )
});

export type EmployeeRequest = z.infer<typeof employeeRequestSchema>;