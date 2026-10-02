import shifterLogo from '@/assets/shifter-logo-v1.svg';

interface StandardLayoutProps {
    children: React.ReactNode;
}

export const StandardLayout = ({ children }: StandardLayoutProps) => {
    return (
        <div className="relative flex flex-col min-h-screen items-center justify-center bg-teal-400">
            <div className="absolute inset-0 z-0 overflow-hidden">
                <div className="absolute -top-24 right-0 h-[150%] w-1/2 -skew-x-12 translate-x-1/3 bg-teal-700/50"></div>
                <div className="absolute -bottom-24 left-0 h-[150%] w-1/3 -skew-x-12 -translate-x-1/2 bg-teal-500/20"></div>
            </div>

            <div className="relative z-10 flex flex-col items-center justify-center w-full max-w-xl h-full bg-white p-8 rounded-2xl -mt-40 m-3 shadow-2xl">

                <a href="#" className="justify-center mb-12">
                    <img src={shifterLogo} alt="Shifter Logo" className="h-8 w-auto" />
                </a>

                {children}

            </div>
        </div>
    );
}