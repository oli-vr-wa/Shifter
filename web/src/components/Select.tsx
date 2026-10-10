import { cn } from "@/lib/utils";
import React from "react";

const Select = React.forwardRef<HTMLSelectElement, React.SelectHTMLAttributes<HTMLSelectElement>>(
    ({ children, className, ...props }, ref) => {
        return (
            <select 
                ref={ref} 
                className={cn(
                    "rounded-md border border-gray-300 p-2 " +
                    "focus:border-teal-500 focus:outline-hidden focus:ring-1 focus:ring-teal-500 " +
                    "disabled:bg-gray-100 disabled:text-gray-500 disabled:cursor-not-allowed " +
                    "aria-invalid:border-red-500 aria-invalid:focus:border-red-500 aria-invalid:focus:ring-red-500", 
                    className
                )} 
                {...props}
            >
                {children}
            </select>
        );
    }
);

export default Select;