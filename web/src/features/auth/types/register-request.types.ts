import { z } from "zod";
import { abnSchema } from "@/shared/validation/abn";

export const registerRequestSchema = z.object({
    firstName: z.string().min(1, { message: "First name is required" }),
    lastName: z.string().min(1, { message: "Last name is required" }),
    email: z.email(),
    password: z.string()
        .min(8, { message: "Password must be at least 8 characters long" })
        .refine((value) => /[A-Z]/.test(value), { message: "Password must contain at least one uppercase letter" })
        .refine((value) => /[a-z]/.test(value), { message: "Password must contain at least one lowercase letter" })
        .refine((value) => /[0-9]/.test(value), { message: "Password must contain at least one number" })
        .refine((value) => /[^A-Za-z0-9]/.test(value), { message: "Password must contain at least one special character" }),
    confirmPassword: z.string().min(1, { message: "Confirm password is required" }),
    companyName: z.string().min(1, { message: "Company name is required" }),
    companyAbn: abnSchema,
}).refine((data) => data.password === data.confirmPassword, {
    message: "Passwords do not match",
    path: ["confirmPassword"],
});

export type RegisterRequest = z.infer<typeof registerRequestSchema>;