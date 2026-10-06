export interface Employee {
    id: string;
    firstName: string;
    lastName: string;
    position: string;
    email: string;
    phone: string;
    dateOfBirth: string;
    baseHourlyRate: number;
    status: 'available' | 'assigned' | 'onLeave';
}