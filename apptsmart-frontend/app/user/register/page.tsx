"use client"

import { useState } from "react";
import { useRouter } from "next/navigation";
import { useUser } from "@/stores/UserContext";
import { fetchBackend } from "@/utilities/helpers";


const Register = () => {
    const user = useUser();
    const router = useRouter();
    const [ firstName, setFirstName ] = useState("");
    const [ lastName, setLastName ] = useState("");
    const [ email, setEmail ] = useState("");
    const [ password, setPassword ] = useState("");

    const [ error, setError ] = useState("");

    const registerUser = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();

        const body = JSON.stringify({ firstName, lastName, email, password });

        const response = await fetchBackend("/auth/register", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: body
        });

        if (!response.ok) {
            setError("Error registering user");
            return;
        }

        // for later, assign/login user after successful registration
        user.login(email, password);
        router.push("/");
    };

    return (
        <>
            <h1>Register for AptSmart!</h1>
            {error ?? <h2 style={{ color: "red"}}>{error}</h2>}
            <form onSubmit={registerUser}>
                <input type="text" value={firstName} placeholder="First Name" required onChange={(e) => setFirstName(e.target.value)} />
                <input type="text" value={lastName} placeholder="Last Name" required onChange={(e) => setLastName(e.target.value)}/>
                <input type="email" value={email} placeholder="Email" required onChange={(e) => setEmail(e.target.value)} />
                <input type="password" value={password} placeholder="Password" required onChange={(e) => setPassword(e.target.value)} />
                <button type="submit">Submit</button>
            </form>
        </>
    );
}
 
export default Register;