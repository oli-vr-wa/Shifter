import { z } from "zod";

export const clientContactSchema = z.object({
    id: z.uuid().optional(),
    firstName: z.string().trim().min(1, "First name is required"),
    lastName: z.string().trim().min(1, "Last name is required"),
    jobTitle: z.string().trim().nullish(),
    email: z.email("Invalid email address").or(z.literal("")).nullish(),
    phoneNumber: z.string().trim().nullish()
}).refine((contact) => !!contact.email || !!contact.phoneNumber, {
    message: "Either email or phone number is required",
    path: ["email"],
});

export type ClientContact = z.infer<typeof clientContactSchema>;