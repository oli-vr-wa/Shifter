import { authService } from "../api/auth.service";
import { createContext, useContext, useEffect, useState } from "react";
import type { User } from "../types/user.types";

const AuthContext = createContext<{
    user: User | null;
    isAuthenticated: boolean;
    isLoading: boolean;
    checkAuthStatus: () => Promise<void>;
    setAuthUser: (userData: User | null) => void;
    logout: () => void;
} | null>(null);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [user, setUser] = useState<User | null>(null);
    const [isAuthenticated, setAuthenticated] = useState(false);
    const [isLoading, setLoading] = useState(true);

    useEffect(() => {
        checkAuthStatus();
    }, []);

    const checkAuthStatus = async () => {
        try {
            const response = await authService.me();
            if (response.status === 200) {
                setAuthUser(response.data);
            } else {
                setAuthUser(null);
            }
        } catch (error) {
            setAuthUser(null);
        } finally {
            setLoading(false);
        }
    };

    const setAuthUser = (userData: User | null) => {
        if (!userData) {
            setUser(null);
            setAuthenticated(false);
            return;
        }
        setUser(userData);
        setAuthenticated(true);
    };

    const logout = () => {
        try {
            authService.logout();
        } catch (error) {
            console.error("Logout failed:", error);
        } finally {
            setUser(null);
            setAuthenticated(false);
        }
    };

    return (
        <AuthContext.Provider value={{ user, isAuthenticated, isLoading, checkAuthStatus, setAuthUser, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthProvider");
    }
    return context;
};