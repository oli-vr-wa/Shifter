import { z } from "zod";

export const TwoFactorAuthenticationRequestSchema = z.object({
    code: z.string()
        .length(6, { message: "Code must be 6 digits" })
        .regex(/^\d+$/, { message: "Code must be numeric" }),
});

export type TwoFactorAuthenticationRequest = z.infer<typeof TwoFactorAuthenticationRequestSchema>;