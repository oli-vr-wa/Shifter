export interface LoginResponse {
    requiresMfa: boolean;
    mfaToken: string;
    setupRequired: boolean;
    manualEntryKey?: string | null;
    qrCodeUri?: string | null;
}