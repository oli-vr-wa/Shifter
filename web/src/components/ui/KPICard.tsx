import type { ReactNode } from "react";

interface KPICardProps {
    title: string;
    value: string | number;
    trend?: string;
    icon?: ReactNode;
    themeColor?: 'green' | 'blue' | 'yellow' | 'purple';
    isLoading?: boolean;
}

export const KPICard = ({ title, value, icon, themeColor = 'green', isLoading = false, trend }: KPICardProps) => {
    const colorClasses = {
        green: { bg: 'bg-teal-100', text: 'text-teal-700', trend: 'text-teal-600' },
        blue: { bg: 'bg-blue-100', text: 'text-blue-700', trend: 'text-blue-600' },
        yellow: { bg: 'bg-amber-100', text: 'text-amber-700', trend: 'text-amber-600' },
        purple: { bg: 'bg-purple-100', text: 'text-purple-700', trend: 'text-purple-600' },
    };

    const themeClass = colorClasses[themeColor];

    if (isLoading) {
        return (
            <div className="bg-white rounded-xl shadow-sm border border-slate-200 p-5">
                <p className={`text-sm ${themeClass.bg} ${themeClass.text} mt-4`}>
                    Loading...
                </p>
            </div>
        );
    }

    return (
        <div className="bg-white rounded-xl shadow-sm border border-slate-200 p-5">
            <div className='flex justify-between items-start'>
                <div>
                    <p className="text-sm text-slate-500">{title}</p>
                    <p className="text-3xl font-bold text-slate-900 mt-1">{value}</p>
                </div>

                <div className={`flex w-10 h-10 rounded-lg items-center justify-center ${themeClass.bg} ${themeClass.text}`}>
                    {icon}
                </div>
            </div>       

            <p className={`text-sm ${themeClass.trend} mt-4`}>
                {trend}
            </p>
        </div>
    )
}