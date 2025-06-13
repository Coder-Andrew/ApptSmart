"use client"

import { fetchBackend } from "@/utilities/helpers";
import { LoginError, LoginErrorCode } from "@/utilities/loginError";
import { createContext, useContext, useEffect, useState } from "react";

interface User {
    id: string,
    email: string,
    roles: Array<string>
};

interface UserContextType {
    user: User | null;
    refreshUser: () => Promise<void>;
    logout: () => Promise<void>;
    login: (email: string, password: string) => Promise<void>;
    authReady: boolean,
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider = ({ children } : { children: React.ReactNode }) => {
    const [ user, setUser ] = useState<User | null>(null);
    const [ authReady, setAuthReady ] = useState<boolean>(false);

    const fetchUser = async () => {
        try {
            setAuthReady(false);
            const response = await fetchBackend("/auth/me",{
                method: "GET",
            });

            if (!response.ok) {
                setUser(null);
            } else {
                const jsonResponse = await response.json();
                setUser(jsonResponse);
            }
        } catch (err) {
            console.log(err);
            setUser(null);
        } finally {
            setAuthReady(true);
        }
    };

    const logout = async () => {
        const response = await fetchBackend("/auth/logout",{
            method: "GET",
        });

        setUser(null);
        return;
    };

    const login = async (email: string, password: string) => {
        const response = await fetchBackend("/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password }),
        });

        if (!response.ok) {
            setUser(null);
            switch (response.status) {
                case 401:
                    throw new LoginError("Incorrect email or password.", LoginErrorCode.INVALID_CREDENTIALS);
                case 429:
                    throw new LoginError("Too many login attempts. Try again later.", LoginErrorCode.TOO_MANY_ATTEMPTS);
                case 500:
                    throw new LoginError("Server error. Please try again later.", LoginErrorCode.SERVER_ERROR);
                default:
                    const errorText = await response.text();
                    throw new LoginError(errorText || "Unknown error occured, please try again later.", LoginErrorCode.UNKNOWN_ERROR);
            }
        }

        fetchUser();
    };

    useEffect(() => {
        fetchUser();
    }, []);

    return (
        <UserContext.Provider value={{ user, refreshUser: fetchUser, logout, login, authReady }}>
            { children }
        </UserContext.Provider>
    );
};

export const useUser = () => {
    const context = useContext(UserContext);
    if (!context) throw new Error("useUser must be used within a UserProvider");
    return context;
}