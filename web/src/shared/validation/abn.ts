import { z } from "zod";

const ABN_WEIGHTS = [10, 1, 3, 5, 7, 9, 11, 13, 15, 17, 19];

export const stripNonDigits = (value: string) => value.replace(/\D/g, "");

export function isValidAbn(value: string): boolean {
    const digits = stripNonDigits(value);
    if (digits.length !== 11) return false;

    const nums = digits.split("").map(Number);
    nums[0] -= 1;
    const sum = nums.reduce((acc, digit, idx) => acc + digit * ABN_WEIGHTS[idx], 0);
    return sum % 89 === 0;
}

// Display helper: "12345678901" -> "12 345 678 901"
export function formatAbn(value: string): string {
    const d = stripNonDigits(value);
    return `${d.slice(0, 2)} ${d.slice(2, 5)} ${d.slice(5, 8)} ${d.slice(8, 11)}`.trim();
}

// Required ABN (company registration)
export const abnSchema = z
    .string()
    .transform(stripNonDigits)
    .pipe(
        z
            .string()
            .min(1, "ABN is required")
            .refine(isValidAbn, "Invalid ABN")
    );

// Optional ABN (clients): empty/null/undefined are allowed, anything else must be valid
export const optionalAbnSchema = z
    .union([z.literal(""), z.null(), z.undefined(), abnSchema])
    .transform((value) => value || undefined);