import { cn } from "@/lib/utils";

export default function Button({ className, ...props }: React.ComponentProps<'button'>) {
    return (
        <button
            className={cn("rounded-md bg-teal-900 px-4 py-2 font-medium text-white hover:bg-teal-700 disabled:cursor-not-allowed disabled:bg-teal-300 cursor-pointer", className)}
            {...props}
        />
    );
}