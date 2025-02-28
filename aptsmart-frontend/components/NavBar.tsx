"use client"
import { useUser } from "@/stores/UserContext";
import Link from "next/link";


const NavBar = () => {
    const user = useUser();
    
    return ( 
        <nav>
            {user.user ? 
            <div>
                <span>Welcome, {user.user.email}</span>
                <span onClick={user.logout}>Logout</span>
            </div> :
            <div>
                <span><Link href={"/user/login"}>Login</Link></span>
                /
                <span><Link href={"/user/register"}>Register</Link></span>
            </div>}
            <hr />
        </nav>
    );
}



export default NavBar;