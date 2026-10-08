import { Badge } from "@/components/ui/Badge";
import { DataTable, type ColumnDef } from "@/components/ui/DataTable";
import type { Employee } from "@/types/models/employee";
import { CalendarIcon, EnvelopeIcon, PencilSquareIcon, PhoneIcon } from "@heroicons/react/24/outline";
import { useEffect, useState } from "react";
import { employeeService } from "../api/employee.service";

type employeeListItem = Pick<Employee, 'id' | 'firstName' | 'lastName' | 'position' | 'email' | 'phone' | 'status'>;

export const CurrentEmployeesTable = () => {
    const [employees, setEmployees] = useState<employeeListItem[]>([]);

    const statusConfig = {
        'assigned' : { badgeType: 'warning', statusDisplay: 'Assigned' },
        'available' : { badgeType: 'success', statusDisplay: 'Available' },
        'onLeave' : { badgeType: 'error', statusDisplay: 'On Leave' }        
    } as const;

    useEffect(() => {
        const fetchCurrentEmployees = async () => {
            const mockEmployees: employeeListItem[] = [
                { id: '1', firstName: "Oliver", lastName: "Ramos", position: "Senior Developer", email: "oliver.v@test.com", phone: "0412 345 678", status: "assigned" },
                { id: '2', firstName: "Sarah", lastName: "Chen", position: "Maintenance Technician", email: "sarah.c@test.com", phone: "0498 765 432", status: "available" },
                { id: '3', firstName: "James", lastName: "Wilson", position: "Project Manager", email: "james.w@test.com", phone: "0423 456 789", status: "assigned" },
                { id: '4', firstName: "Emily", lastName: "Davis", position: "UX Designer", email: "emily.d@test.com", phone: "0422 888 555", status: "onLeave" },
            ];

            try {
                const currentEmployees = await employeeService.getCurrent();
                setEmployees(currentEmployees.data);
            } catch {
                setEmployees(mockEmployees);
            }
        };

        void fetchCurrentEmployees();
    }, []);

    const columns: ColumnDef<employeeListItem>[] = [
        {
            header: 'AVATAR',
            cell: (row) => (
                <div className="flex items-center">
                    <div className="w-10 h-10 rounded-full flex items-center justify-center font-medium text-sm bg-teal-100 text-teal-700">
                        {row.firstName.charAt(0)}{row.lastName.charAt(0)}
                    </div>
                </div>
            )
        },
        {
            header: 'EMPLOYEE',
            cell: (row) => <div className="font-medium text-gray-900">{row.firstName} {row.lastName}</div>
        },
        {
            header: 'POSITION',
            cell: (row) => <div className="font-sm text-gray-900">{row.position}</div>
        },
        {
            header: 'CONTACT',
            cell: (row) => (
                <div className="flex flex-col gap-1">
                    <div className="flex items-center gap-2 text-sm text-gray-600"><EnvelopeIcon className="w-4 h-4" /> {row.email}</div>
                    <div className="flex items-center gap-2 text-sm text-gray-600"><PhoneIcon className="w-4 h-4" /> {row.phone}</div>
                </div>
            )   
        },
        {
            header: 'STATUS',
            cell: (row) => {row.status && <Badge type={statusConfig[row.status].badgeType}>{statusConfig[row.status].statusDisplay}</Badge>}
        },
        {
            header: 'ACTIONS',
            className: 'text-right',
            cell: () => (
                <div className="flex items-center justify-end gap-3">
                    <button className="text-gray-400 hover:text-teal-600 cursor-pointer"><CalendarIcon className="w-5 h-5" /></button>
                    <button className="text-gray-400 hover:text-teal-600 cursor-pointer"><PencilSquareIcon className="w-5 h-5" /></button>
                </div>
            )
        }
    ];

    return <DataTable 
        columns={columns}
        data={employees}
        showToolbar={true}
        searchPlaceholder="Search employee..."
        showFilters={true}
        onFilterClick={() => console.log('Open filter modal')} />
};
