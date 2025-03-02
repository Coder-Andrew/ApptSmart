"use client"

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
            const response = await fetch("/api/auth/me",{
                method: "GET",
                credentials: "include"
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
        const response = await fetch("/api/auth/logout",{
            method: "GET",
            credentials: "include"
        });

        setUser(null);
        return;
    };

    const login = async (email: string, password: string) => {
        const response = await fetch("/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password }),
            credentials: "include"
        });

        console.log(response);

        if (!response.ok) {
            console.error("Error logging user in");
            setUser(null);
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