import { cn } from "@/lib/utils";

const themes = {
    'primary': "bg-teal-900 text-white hover:bg-teal-700 disabled:bg-teal-500",
    'secondary': "bg-white text-gray-700 hover:bg-gray-100 disabled:bg-gray-300 border border-gray-200",
    'danger': "bg-red-600 text-white hover:bg-red-500 disabled:bg-red-300",
    'warning': "bg-yellow-500 text-white hover:bg-yellow-400 disabled:bg-yellow-300",
    'info': "bg-blue-500 text-white hover:bg-blue-400 disabled:bg-blue-300",
};

type ButtonProps = React.ComponentProps<'button'> & {
    theme?: keyof typeof themes;
};

export default function Button({ theme = "primary", className, ...props }: ButtonProps) {
    return (
        <button
            className={cn("rounded-md px-4 py-2 font-medium disabled:cursor-not-allowed transition-colors cursor-pointer", themes[theme], className)}
            {...props}
        />
    );
}