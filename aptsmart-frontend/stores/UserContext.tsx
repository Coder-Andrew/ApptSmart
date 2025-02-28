"use client"

import { createContext, useContext, useEffect, useState } from "react";

interface User {
    id: string,
    email: string,
    role: Array<string>
};

interface UserContextType {
    user: User | null;
    refreshUser: () => Promise<void>;
    logout: () => Promise<void>;
    login: (email: string, password: string) => Promise<void>;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider = ({ children } : { children: React.ReactNode }) => {
    const [user, setUser] = useState<User | null>(null);

    const fetchUser = async () => {
        try {
            const response = await fetch("/api/auth/me",{
                method: "GET",
                credentials: "include"
            });

            if (!response.ok) {
                setUser(null);
                return;
            }

            const jsonResponse = await response.json();

            console.log(jsonResponse);

            setUser(jsonResponse);
        } catch (err) {
            console.log(err);
            setUser(null);
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
        <UserContext.Provider value={{ user, refreshUser: fetchUser, logout, login }}>
            { children }
        </UserContext.Provider>
    );
};

export const useUser = () => {
    const context = useContext(UserContext);
    if (!context) throw new Error("useUser must be used within a UserProvider");
    return context;
}