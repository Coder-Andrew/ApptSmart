"use client"
import Link from "next/link";
import styles from "./RegisterModal.module.css"
import Image from "next/image";
import { useState } from "react";

const RegisterModal = () => {
    const [ showModal, setShowModal ] = useState(true);

    return (
        showModal && (
            <>
                <div className={styles.modalBackFade}></div>
                <div className={styles.modalBackground}>
                <div className={styles.modalInfoCard}>
                    <h1 className={styles.primaryHeader}>
                        Let us handle all of your appointment scheduling needs.
                    </h1>
                    <h2 className={styles.secondaryHeader}>
                        Registration is quick and simple
                    </h2>
                    {/* <img src="./Logo.webp"></img> */}
                </div>
                <div className={styles.modalInputGroup}>
                    <div>
                        <h1 className={styles.primaryHeader}>
                            Get Started
                        </h1>
                        <h2 className={styles.secondaryHeader}>
                            Create your account now
                        </h2>
                    </div>
                    <form>
                        <div className={styles.formInput}>                            
                            <label>First Name</label>
                            <input placeholder="First name"></input>
                        </div>
                        <div className={styles.formInput}>                            
                            <label>Last Name</label>
                            <input placeholder="Last name"></input>
                        </div>
                        <div className={styles.formInput}>                            
                            <label>Email</label>
                            <input placeholder="Email"></input>
                        </div>
                        <div className={styles.formInput}>                            
                            <label>Password</label>
                            <input placeholder="Password"></input>
                        </div>
                        <button className={styles.submitFormBtn}>Sign Up</button>
                    </form>
                    <div className={styles.existingAccount}>
                        <span>Have an account?</span>
                        <Link href={"/login"} className={styles.loginBtn}>Login</Link>
                    </div>
                </div>
            </div>
        </>)
    );
}
 
export default RegisterModal;