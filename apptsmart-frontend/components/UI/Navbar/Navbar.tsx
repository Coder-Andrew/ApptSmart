"use client"
import { useUser } from "@/stores/UserContext";
import { usePathname } from "next/navigation";
import styles from "./Navbar.module.css";
import Link from "next/link";
import { useModal } from "@/providers/ModalProvider";
import Logo from "../Logo/Logo";
import ROUTES from "@/lib/routes";
import { RxHamburgerMenu } from "react-icons/rx";
import { useEffect, useState } from "react";
import ProtectedLinkButton from "../Auth/ProtectedLink";


const Navbar = () => {
    const { user, logout, authReady } = useUser();
    const { openModal } = useModal();

    const [ dropdown, setDropdown ] = useState(true);
    const [ isMobile, setIsMobile ] = useState(false);
    const closeDropdown = () => { 
        if (isMobile) setDropdown(false);
    };

    useEffect(() => {
        const handleResize = () => {
            const mobile = window.innerWidth < 768;
            setIsMobile(mobile);
            setDropdown(!mobile);
        };

        handleResize();
        window.addEventListener("resize", handleResize);

        return () => {
            window.removeEventListener("resize", handleResize);
        }
    }, []);

    const generateProfileColor = (letter: string) => {
        // TODO: figure out how to support UTF-8/16/Unicode/non-english chars
        // This is some terrible code, you need to rewrite this
        const char = letter.charCodeAt(0) - 65;
        let colorTotal = char * 30;
        const c1 = formatColor(clampColor(colorTotal).toString(16));
        colorTotal -= 255;
        const c2 = formatColor(clampColor(colorTotal).toString(16));
        colorTotal -= 255;
        const c3 = formatColor(clampColor(colorTotal).toString(16));
        return '#' + c1 + c2 + c3;
    };
    const clampColor = (colorValue: number) => {
        if (colorValue < 0) return 0;
        else if (colorValue > 255) return 255;
        else return colorValue;
    }
    const formatColor = (value: string) => {
        if (value === '0') return '00';
        if (value.length === 1) return '0' + value;
        return value;
    };
    
    const pathname = usePathname();

    const navLinks = [
        { name: "Home", path: "/"},
        { name: "Appointments", path: ROUTES.appointments, protected: true },
        { name: 'Company Registration', path: ROUTES.companyRegistration, protected: true },
        { name: "Contact Us", path: ROUTES.contactUs }
    ];

    return (
        <nav className={`container ${styles.navbar}`}>
            <div className={styles.collapsible}>
                <div className={styles.logoGroup}>
                    <Logo className={styles.logoSvg}/>
                    <Link href={"/"} className={`font-logo text-secondary ${styles.logoText}`}>ApptSmart</Link>
                </div>
                {/* <label htmlFor={styles.menuToggle}><RxHamburgerMenu className={styles.hamBurger} /></label>
                <input type="checkbox" id={styles.menuToggle} className={styles.menuToggle} hidden /> */}
                <RxHamburgerMenu className={styles.hamBurger} onClick={() => setDropdown(!dropdown)}/>
            </div>
            { dropdown &&
            <>
                <div className={styles.navLinkGroup}>
                    { navLinks.map(nl => (
                        !nl.protected ?
                        <Link 
                            key={nl.name}
                            href={nl.path}
                            className={pathname === nl.path ? "text-secondary" : "text-muted"}
                            onClick={()=>closeDropdown()}
                        >
                            {nl.name}
                        </Link> :
                        <ProtectedLinkButton key = {nl.name} href={nl.path} className={pathname === nl.path ? "text-secondary" : "text-muted"}>
                            {nl.name}
                        </ProtectedLinkButton>

                    ))}
                </div>
                <div className={`${authReady ? "visible" : "invisible" } ${styles.navUserGroup}`}>
                    { user ? (
                        // <p className="cursor-pointer text-secondary">{ user.email }</p>
                        // Left off trying to add a circle with user email's first letter
                        <svg className="cursor-pointer" width={50} height={50} onClick={logout}>
                            <circle fill={generateProfileColor(user.email[0].toUpperCase())} cx="50%" cy="50%" r={25}/>
                            <text fill="black" x="50%" y="50%" dominantBaseline="middle" textAnchor="middle" fontSize={35}>{user.email[0].toUpperCase()}</text>
                        </svg>
                    ) : (
                        <>
                            <button className={`button-ghost`} onClick={()=>{closeDropdown(); openModal('login')}}>Login</button>
                            <button className={`button-tertiary`} onClick={()=>{closeDropdown(); openModal('register')}}>Register</button>
                        </>
                    )}
                </div>
            </>
            }
        </nav>
    )
}



export default Navbar;