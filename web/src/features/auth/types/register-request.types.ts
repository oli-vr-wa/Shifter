import { z } from "zod";

const abnWeights = [10, 1, 3, 5, 7, 9, 11, 13, 15, 17, 19]

const abnSchema =  z
    .string()    
    .default("")
    .transform((val) => val.replace(/\D/g, ""))
    .pipe(                
        z.string()
            .refine(
            (val) => {
                if (val.length !== 11) return false;
                const digits = val.split("").map(Number);
                digits[0] -= 1;
                const sum = digits.reduce((acc, digit, idx) => acc + digit * abnWeights[idx], 0);
                return sum % 89 === 0;
            },
            { message: "Invalid ABN" }
            )
            .transform((val) => `${val.slice(0, 2)} ${val.slice(2, 5)} ${val.slice(5, 8)} ${val.slice(8, 11)}`)
);

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