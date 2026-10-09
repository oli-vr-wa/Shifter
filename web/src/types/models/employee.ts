export interface Employee {
    id: string;
    firstName: string;
    lastName: string;
    jobPosition: string;
    email: string;
    phoneNumber: string;
    dateOfBirth: string;
    baseHourlyRate: number;
    employmentStartDate: string;
    status: 'available' | 'assigned' | 'onLeave';
    role: 0 | 1 | 2;
}