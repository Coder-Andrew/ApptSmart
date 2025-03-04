"use client"
import { useUser } from "@/stores/UserContext";
import { usePathname } from "next/navigation";
import styles from "./Navbar.module.css";
import Link from "next/link";


const Navbar = () => {
    //const { user, logout, authReady } = useUser();
    
    const pathname = usePathname();
    console.log(pathname);

    const navLinks = [
        { name: "Home", path: "/" },
        { name: "Appointments", path: "/appointments" },
        { name: "About", path: "/about" },
        { name: "Contact Us", path: "/contact-us" }
    ];

    return (
        <nav className={styles.navbar}>            
            <div className={styles.logo}>
                <Link href={"/"}>
                    <span className={styles.logoText}>Appt</span>
                    <span className={styles.logoHighlight}>Smart</span>
                    <span className={styles.logoIcon}>📅</span>
                </Link>
            </div>
            <div className={styles.navLinks}>
                {navLinks.map((link) => (
                    <Link
                        key={link.path}
                        href={link.path}
                        className={pathname === link.path ? styles.active : ""}
                    >
                        {link.name}
                    </Link>
                ))}
            </div>
            <div className={styles.userGroup}>
                <Link className={styles.login} href={"/login"}>Sign In</Link>
                <Link className={styles.register} href={"/register"}>Sign Up</Link>
            </div>
        </nav>
    )
}



export default Navbar;