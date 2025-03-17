"use client"
import Link from "next/link";
import styles from "./RegisterModal.module.css"
import Image from "next/image";
import { Calendar } from "lucide-react";
import { useModal } from "@/providers/ModalProvider";

const RegisterModal = () => {
    const { closeModal, isModalOpen } = useModal();

    const modalName = "register";

    if (!isModalOpen(modalName)) return null;

    const inputGroup = [
        { name: "First Name", type: "text" },
        { name: "Last Name", type: "text" },
        { name: "Email", type: "email" },
        { name: "Password", type: "password"}
    ];

    return (
        <div className={styles.modalOverlay} onClick={() => closeModal(modalName)}>
            <div className={styles.modalContainer} onClick={(e) => e.stopPropagation()}>
                <div className={styles.modalLeft}>
                    <div className={styles.calendarIconSmall}>
                        <Calendar className="w-6 h-6 text-gray-700" />
                    </div>
                    <h2 className={styles.modalTitle}>Let us handle all of your appointment scheduling needs.</h2>
                    <p className={styles.modalSubTitle}>Registration is quick and simple</p>
                    <div className={styles.calendarIconLarge}>
                        <div className={styles.calendarIconCircle}>
                            <svg
                                viewBox="0 0 24 24"
                                width="40"
                                height="40"
                                stroke="currentColor"
                                strokeWidth="2"
                                fill="none"
                                strokeLinecap="round"
                                strokeLinejoin="round"
                            >
                                <rect x="3" y="4" width="18" height="18" rx="2" ry="2"></rect>
                                <line x1="16" y1="2" x2="16" y2="6"></line>
                                <line x1="8" y1="2" x2="8" y2="6"></line>
                                <line x1="3" y1="10" x2="21" y2="10"></line>
                                <circle cx="8" cy="14" r="1"></circle>
                                <circle cx="12" cy="14" r="1"></circle>
                                <circle cx="16" cy="14" r="1"></circle>
                                <circle cx="8" cy="18" r="1"></circle>
                                <circle cx="12" cy="18" r="1"></circle>
                                <circle cx="16" cy="18" r="1"></circle>
                            </svg>
                        </div>
                    </div>
                </div>
                <div className={styles.modalRight}>
                    <button className={styles.closeButton} onClick={() => closeModal(modalName)}>&times;</button>
                    <h2 className={styles.formTitle}>Get started</h2>
                    <p className={styles.formSubTitle}>Create your account now</p>
                    <form className={styles.signupForm}>
                        {inputGroup.map((i) => (
                            <div key={i.name} className={styles.formGroup}>
                                <label>{i.name}</label>
                                <input type={i.type} placeholder={i.name}/>
                            </div>
                        ))}
                        <button type="submit" className={styles.signupButton}>Sign Up</button>
                    </form>
                    <div className={styles.loginLink}>
                        <span>Have an account?</span>
                        <Link href={"/user/login"}>Login</Link>
                    </div>
                </div>
            </div>
        </div>
    );
}
 
export default RegisterModal;