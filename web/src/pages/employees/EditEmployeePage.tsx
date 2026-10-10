import { useNavigate, useParams } from "react-router-dom";
import { EmployeeForm } from "@/features/employees";
import { employeeService } from "@/features/employees/api/employee.service";
import type { EmployeeRequest, UserRole } from "@/features/employees/types";
import { useQuery } from "@tanstack/react-query";
import { toInputDate } from "@/lib/utils/dateHelpers";

export default function EditEmployeePage() {
    const { employeeId } = useParams<{ employeeId: string }>();
    //const [employeeData, setEmployeeData] = useState<EmployeeRequest | null>(null);
    const navigate = useNavigate();

    const { data: employeeData, isLoading, isError } = useQuery({
        queryKey: ['employee', employeeId],
        queryFn: async () => (await employeeService.getById(employeeId!)).data,
        enabled: !!employeeId,
    });

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (isError || !employeeData) {
        return <div>Error loading employee data.</div>;
    }

    const initialData: EmployeeRequest = {
        id: employeeData?.id ?? '',
        firstName: employeeData?.firstName ?? '',
        lastName: employeeData?.lastName ?? '',
        email: employeeData?.email ?? '',
        phoneNumber: employeeData?.phoneNumber ?? '',
        jobPosition: employeeData?.jobPosition ?? '',
        employmentStartDate: toInputDate(employeeData?.employmentStartDate ?? ''),
        role: employeeData?.role ?? '' as unknown as UserRole,
    };

    return (
        <div className="w-full max-w">
            <EmployeeForm
                initialData={initialData}
                onSuccess={() => navigate('/employees')}
                onCancel={() => navigate('/employees')}                
            />
        </div>
    )
}