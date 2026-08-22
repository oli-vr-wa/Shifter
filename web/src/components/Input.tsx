import { cn } from "@/lib/utils";
import React from "react";

const Input = React.forwardRef<HTMLInputElement, React.InputHTMLAttributes<HTMLInputElement>>(
    ({ className, type = 'text', autoComplete = 'on', ...props }, ref) => {

    return (
        <input
            ref={ref}
            type={type}
            className={cn(
                "rounded-md border border-gray-300 p-2 " +
                "focus:border-teal-500 focus:outline-hidden focus:ring-1 focus:ring-teal-500 " +
                "disabled:bg-gray-100 disabled:text-gray-500 disabled:cursor-not-allowed " +
                "aria-invalid:border-red-500 aria-invalid:focus:border-red-500 aria-invalid:focus:ring-red-500", 
                className)}
            autoComplete={autoComplete}
            {...props}
        />
    );
});

export default Input;