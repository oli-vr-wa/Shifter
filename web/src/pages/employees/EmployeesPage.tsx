import { CurrentEmployeesTable, EmployeesSummary } from '@/features/employees';
import { PlusIcon } from '@heroicons/react/24/outline';

export default function EmployeesPage() {

  return (
    <div className="w-full max-w">
      {/* Page Header */}
      <div className="flex justify-between items-start mb-8">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 mb-1">Employees</h1>
          <p className="text-gray-500 text-sm">Manage your workforce, contact details, and availability.</p>
        </div>
        <button className="flex items-center gap-2 bg-[#0d3330] hover:bg-[#154d48] text-white px-4 py-2.5 rounded-lg text-sm font-medium transition-colors">
          <PlusIcon className="w-5 h-5" />
          Add Employee
        </button>
      </div>

      {/* Summary Cards */}
      <EmployeesSummary />      

      {/* Main Content Area */}
      <CurrentEmployeesTable />

    </div>
  );
}