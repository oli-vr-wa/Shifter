import { KPICard, type KPICardProps } from "@/components/ui/KPICard";
import type { EmployeesMetrics } from "../types";
import { useEffect, useState } from "react";

export const EmployeesSummary = () => {
    const [cardsData, setCardsData] = useState<KPICardProps[]>([]);

    useEffect(() => {
        const mockData: EmployeesMetrics = {
            total: 4,
            available: 3,
            onLeave: 1,
        }
        
        setCardsData([
            { title: "Total Employees", value: mockData.total },
            { title: "Available Today", value: mockData.available },
            { title: "On Leave", value: mockData.onLeave },
        ]);
    }, []);

    return (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
            {cardsData.map((card, index) => (
                <KPICard key={index} title={card.title} value={card.value} />
            ))}
        </div>
    )
}