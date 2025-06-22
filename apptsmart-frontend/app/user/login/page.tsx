"use client"

import { useUser } from "@/stores/UserContext";
import { useRouter } from "next/navigation";
import { useState } from "react";

const Login = () => {
    const router = useRouter();
    const user = useUser();
    const [ email, setEmail ] = useState("");
    const [ password, setPassword ] = useState("");

    const handleLogin = async (e: React.FormEvent<HTMLElement>) => {
        e.preventDefault();
        await user.login(email, password);
        router.push("/");
    };

    return (  
        <>
            <h1>Login into your AptSmart account!</h1>
            <form onSubmit={handleLogin}>
                {/* TODO: Change input type to email and password */}
                <input type="text" required value={email} placeholder="Email" onChange={(e) => {setEmail(e.target.value)}} />
                <input type="text" required value={password} placeholder="Password" onChange={(e) => {setPassword(e.target.value)}} />
                <button type="submit">Login</button>
            </form>
            <button onClick={() => user.logout()}>Logout</button>
        </>
    );
}
 
export default Login;