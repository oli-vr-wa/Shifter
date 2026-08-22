import { cn } from "@/lib/utils";

export default function Link({ className, ...props }: React.ComponentProps<'a'>) {
    return (
        <a
            className={cn("text-teal-600 hover:underline text-sm", className)}
            {...props}
        />
    );
}