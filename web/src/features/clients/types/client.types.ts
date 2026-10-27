import { z } from "zod";
import { addressSchema } from "@/shared/types";
import { clientContactSchema } from "./client-contact.types";
import { abnSchema, optionalAbnSchema } from "@/shared/validation/abn";

export const ClientType = {
    Individual: 0,
    Company: 1
} as const;

export type ClientType = (typeof ClientType)[keyof typeof ClientType];

const baseClientSchema = z.object({
    id: z.uuid().optional(),
    email: z.email("Invalid email address").or(z.literal("")).nullish(),
    phoneNumber: z.string().nullish(),
    address: addressSchema.nullish(),
    contacts: z.array(clientContactSchema).nullish()
});

const individualClientSchema = baseClientSchema.extend({
    type: z.literal(ClientType.Individual),
    firstName: z.string().min(1, "First name is required"),
    lastName: z.string().min(1, "Last name is required"),
    companyName: z.string().nullish(),
    abn: optionalAbnSchema
});

const companyClientSchema = baseClientSchema.extend({
    type: z.literal(ClientType.Company),
    companyName: z.string().min(1, "Company name is required"),
    abn: abnSchema,
    firstName: z.string().nullish(),
    lastName: z.string().nullish()
});

export const clientRequestSchema = z.preprocess(
    (val) => {
        // Select inputs give strings; coerce type to a number
        if (val && typeof val === "object" && "type" in val) {
            const v = val as Record<string, unknown>;
            return { ...v, type: v.type === "" || v.type === undefined ? undefined : Number(v.type) };
        }
        return val;
    },
    z.discriminatedUnion("type", [individualClientSchema, companyClientSchema], {
        message: "Please select a valid client type"
    })
);

export type ClientRequest = z.infer<typeof clientRequestSchema>;