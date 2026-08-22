import React from "react";
import Input from "../Input";

interface FormFieldProps extends React.InputHTMLAttributes<HTMLInputElement> {
    label?: string;
    error?: string;
}

export const FormField = React.forwardRef<HTMLInputElement, FormFieldProps>(({ label, type = 'text', placeholder, error, id, name, ...props }, ref) => {

    const inputId = id || name;
    const isInvalid = !!error;

    return (
        <div className="flex flex-col">
            {label && <label htmlFor={inputId} className="mb-1 text-sm font-medium text-gray-700">{label}</label>}
            <Input ref={ref} id={inputId} name={name} type={type} placeholder={placeholder} aria-invalid={isInvalid} {...props} />
            {error && <span className="mt-1 text-sm text-red-500">{error}</span>}
        </div>    
    );
});