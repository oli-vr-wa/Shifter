import { useNavigate } from "react-router-dom";
import { EmployeeForm } from "@/features/employees";

export default function NewEmployeePage() {
  const navigate = useNavigate();

  return (
    <div className="w-full max-w">
      <EmployeeForm
        onSuccess={() => navigate('/employees')}
        onCancel={() => navigate('/employees')}
      />
    </div>
  );
}