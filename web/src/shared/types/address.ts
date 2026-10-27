import { z } from "zod"

export const addressSchema = z.object({
    placeId: z.string().min(1),
    displayName: z.string().min(1, "Enter a descriptive name"),
    streetNumber: z.string(),
    streetName: z.string(),
    suburb: z.string().min(1),
    postCode: z.string().min(1),
    state: z.string().min(1),
    country: z.string().min(1),
    latitude: z.number().nullish(),
    longitude: z.number().nullish(),
}, 
{
    error: "Select an address from the list"
});

export type Address = z.infer<typeof addressSchema>;