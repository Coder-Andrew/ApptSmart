"use client"
import { useUser } from "@/stores/UserContext";
import Link from "next/link";


const NavBar = () => {
    const { user, logout, authReady } = useUser();
    
    console.log(user);

    return ( 
        <nav>
            { authReady &&
                <div>
                    {user ? (
                        <>
                            <span>Welcome, {user.email}</span>
                            <span onClick={logout}>Logout</span>                        
                            { user.roles.includes("User") && <span>User can see this</span>}
                        </>
                    ) : (
                        <>
                            <span><Link href={"/user/login"}>Login</Link></span>/
                            <span><Link href={"/user/register"}>Register</Link></span>
                        </>
                    )}
                </div>
            }
            <hr />
        </nav>
    );
}



export default NavBar;