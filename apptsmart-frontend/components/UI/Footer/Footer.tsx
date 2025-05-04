import Link from "next/link";
import Logo from "../Logo/Logo";
import styles from "./Footer.module.css";
import ROUTES from "@/lib/routes";

import { FaFacebook } from "react-icons/fa";
import { FaInstagram } from "react-icons/fa";
import { FaTwitter } from "react-icons/fa";


const Footer = () => {
    const iconClasses = `text-tertiary`;
    const iconSize = 20;
    const mediaLinks = [
        { 'icon': <FaFacebook  className={iconClasses} size={iconSize} />, 'href': ROUTES.facebook },
        { 'icon': <FaInstagram className={iconClasses} size={iconSize} />, 'href': ROUTES.instagram },
        { 'icon': <FaTwitter className={iconClasses} size={iconSize} />, 'href': ROUTES.twitter }
    ]

    const navLinks = [
        { 'name': 'Home', 'href': ROUTES.home },
        { 'name': 'Appointments', 'href': ROUTES.appointments },
        { 'name': 'Features', 'href': ROUTES.features },
        { 'name': 'About', 'href': ROUTES.about },
        { 'name': 'Contact Us', 'href': ROUTES.contactUs },
        { 'name': 'Privacy Policy', 'href': ROUTES.privacyPolicy },
        { 'name': 'Terms of Use', 'href': ROUTES.tos },
        { 'name': 'Sitemap', 'href': ROUTES.sitemap },
        { 'name': 'Help & Support', 'href': ROUTES.support },
    ];



    return (
        <div>
            <div className={`bg-footer ${styles.containerBackground}`}>
                <div className={`container ${styles.container}`}>
                    <div className={styles.left}>
                        <Link href={ROUTES.home} className={styles.brand}>
                            <Logo className={styles.logo} circleColorClass={styles.logoMainColor} rectColorClass={styles.logoSecondaryColor}/>
                            <p className="font-logo text-tertiary">ApptSmart</p>
                        </Link>
                        <p className="text-footer">help@apptsmart.com</p>
                        <div className={styles.mediaLinks}>
                            { mediaLinks.map((link, i) => (
                                <Link key={i} href={link.href} >
                                    {link.icon}
                                </Link>
                            ))}
                        </div>
                    </div>
                    <div className={styles.right}>
                        { navLinks.map((link) => (
                            <Link key={link.name} href={link.href} className={`text-footer ${styles.navLink}`}>
                                {link.name}
                            </Link>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
}
 
export default Footer;