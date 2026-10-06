const themeConfig = {
    'default': "bg-blue-100 text-blue-700",
    'success': "bg-teal-100 text-teal-700",
    'warning': "bg-amber-100 text-amber-700",
    'error': "bg-red-100 text-red-700",
    'info': "bg-purple-100 text-purple-700",
}

export const Badge = ({ type = 'default', children }: { type?: keyof typeof themeConfig; children: React.ReactNode }) => {
    const className = themeConfig[type] || themeConfig['default'];
    return (
        <span className={`px-3 py-1 text-xs font-medium rounded-full ${className}`}>
            {children}
        </span>
    );
};