"use client"
import { useUser } from "@/stores/UserContext";
import { usePathname } from "next/navigation";
import styles from "./Navbar.module.css";
import Link from "next/link";
import { useModal } from "@/providers/ModalProvider";


const Navbar = () => {
    const { user, logout, authReady } = useUser();
    const { openModal } = useModal();

    
    const pathname = usePathname();

    const navLinks = [
        { name: "Home", path: "/", loginRequired: false },
        { name: "Appointments", path: "/appointments", loginRequired: true },
        { name: "Contact Us", path: "/contact-us", loginRequired: false }
    ];

    console.log(user);

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
                        href={link.loginRequired && user == null ? "/user/login" : link.path}
                        className={pathname === link.path ? styles.active : ""}
                    >
                        {link.name}
                    </Link>
                ))}
            </div>
            <div className={styles.userGroup}>
                <button className={styles.login}><Link href={"user/login"}>Login</Link></button>
                <button className={styles.register} onClick={() => openModal("register")}>Sign Up</button>
            </div>
        </nav>
    )
}



export default Navbar;