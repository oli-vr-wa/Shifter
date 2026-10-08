import { useNavigate } from "react-router-dom";
import { CurrentEmployeesTable, EmployeesSummary } from '@/features/employees';
import { PlusIcon } from '@heroicons/react/24/outline';
import Button from "@/components/Button";

export default function EmployeesPage() {
  const navigate = useNavigate();

  return (
    <div className="w-full max-w">
      {/* Page Header */}
      <div className="flex justify-between items-start mb-8">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 mb-1">Employees</h1>
          <p className="text-gray-500 text-sm">Manage your workforce, contact details, and availability.</p>
        </div>
        <Button
          className="flex items-center gap-2"
          onClick={() => navigate('/employees/new')}>          
          <PlusIcon className="w-5 h-5" />
          Add Employee
        </Button>
      </div>

      {/* Summary Cards */}
      <EmployeesSummary />      

      {/* Main Content Area */}
      <CurrentEmployeesTable />

    </div>
  );
}