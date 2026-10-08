import React from "react";

interface FormSelectProps extends React.SelectHTMLAttributes<HTMLSelectElement> {
    label: string;    
    options: { label: string; value: string; }[];
    placeholder?: string;
    error?: string | null;
}

export const FormSelect =  React.forwardRef<HTMLSelectElement, FormSelectProps>(({ label, options, placeholder, id, name, error, ...props }: FormSelectProps, ref) => {

    const inputId = id || name;
    const isInvalid = !!error;

    return (
        <div className="flex flex-col gap-1">
            {label && <label htmlFor={inputId} className="text-sm font-medium text-gray-700">{label}</label>}
            <select ref={ref} id={inputId} name={name} className={`border rounded-md p-2 ${isInvalid ? 'border-red-600' : 'border-gray-300'}`} {...props}>
                {placeholder && <option value="">{placeholder}</option>}
                {options.map((option) => (
                    <option key={option.value} value={option.value}>
                        {option.label}
                    </option>
                ))}
            </select>
            {error && <p className="text-sm text-red-600">{error}</p>}
        </div>
    );
});