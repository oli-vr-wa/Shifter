import { Badge } from "@/components/ui/Badge";
import { DataTable, type ColumnDef } from "@/components/ui/DataTable";
import type { Employee } from "@/types/models/employee";
import { CalendarIcon, EnvelopeIcon, PencilSquareIcon, PhoneIcon } from "@heroicons/react/24/outline";
import { useQuery } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import { employeeService } from "../api/employee.service";

type employeeListItem = Pick<Employee, 'id' | 'firstName' | 'lastName' | 'jobPosition' | 'email' | 'phoneNumber' | 'status'>;

export const CurrentEmployeesTable = () => {
    const navigate = useNavigate();

    const handleNavigateToEditEmployee = (employeeId: string) => {
        navigate(`/employees/${employeeId}/edit`);
    };

    const statusConfig = {
        'assigned' : { badgeType: 'warning', statusDisplay: 'Assigned' },
        'available' : { badgeType: 'success', statusDisplay: 'Available' },
        'onLeave' : { badgeType: 'error', statusDisplay: 'On Leave' }        
    } as const;

    const { data: currentEmployeesData, isLoading, isError } = useQuery({
        queryKey: ["currentEmployees"],
        queryFn: () => employeeService.getCurrent(),
    });

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
            cell: (row) => <div className="font-sm text-gray-900">{row.jobPosition}</div>
        },
        {
            header: 'CONTACT',
            cell: (row) => (
                <div className="flex flex-col gap-1">
                    <div className="flex items-center gap-2 text-sm text-gray-600"><EnvelopeIcon className="w-4 h-4" /> {row.email}</div>
                    <div className="flex items-center gap-2 text-sm text-gray-600"><PhoneIcon className="w-4 h-4" /> {row.phoneNumber}</div>
                </div>
            )   
        },
        {
            header: 'STATUS',
            cell: (row) => (row.status && <Badge type={statusConfig[row.status].badgeType}>{statusConfig[row.status].statusDisplay}</Badge>)
        },
        {
            header: 'ACTIONS',
            className: 'text-right',
            cell: (row) => (
                <div className="flex items-center justify-end gap-3">
                    <button className="text-gray-400 hover:text-teal-600 cursor-pointer"><CalendarIcon className="w-5 h-5" /></button>
                    <button 
                        onClick={() => handleNavigateToEditEmployee(row.id)} 
                        className="text-gray-400 hover:text-teal-600 cursor-pointer">
                            <PencilSquareIcon className="w-5 h-5" />
                    </button>
                </div>
            )
        }
    ];

    return <DataTable 
        columns={columns}
        data={currentEmployeesData?.data ?? []}
        isLoading={isLoading}
        isLoadError={isError}
        showToolbar={true}
        searchPlaceholder="Search employee..."
        showFilters={true}
        onFilterClick={() => console.log('Open filter modal')} />
};
