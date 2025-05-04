"use client"
import { useModal } from "@/providers/ModalProvider";
import ModalBase from "../ModalBase";
import styles from "./LoginModal.module.css";
import { FormEvent, useEffect, useState } from "react";
import { FaRegCalendarAlt } from "react-icons/fa";
import ErrorableFormField from "../../ErrorableFormField";
import { useUser } from "@/stores/UserContext";
import { LoginError } from "@/utilities/loginError";
import { Calendar } from "lucide-react";

const LoginModal = () => {
    const { isModalOpen, closeModal, openModal } = useModal();
    const { user, login } = useUser();
    
    const modalName = "login";
    
    const [ email, setEmail ] = useState("");
    const [ password, setPassword ] = useState("");
    const [ error, setError ] = useState("");
    const [ success, setSuccess ] = useState(false);
    
    useEffect(() => {
        setError("");
    },[email, password]);


    if (!isModalOpen(modalName)) return null;

    const loginUser = async (e: FormEvent) => {
        e.preventDefault();
        setEmail("");
        setPassword("");
        try {
            await login(email, password);
            setError('');
            setSuccess(true);
            setTimeout(() => {
                closeModal(modalName)
                setSuccess(false);
            },1500);
        } catch (err) {
            if (err instanceof LoginError) {
                setError(err.message);
            } else {
                setError("An unexpected error occured, please try again later.");
            }
        }
    }

    return (
        <ModalBase modalName={modalName}>
            <div className={`bg-background ${styles.container}`}>
                <div className={`bg-tertiary ${styles.left}`}>
                    <h1 className={`font-primary`}>Login to ApptSmart!</h1>
                    <div className={styles.calendarIconSmall}>
                        <Calendar />
                    </div>
                </div>
                <div className={`bg-background ${styles.right}`}>
                    <p className={`cursor-pointer text-primary ${styles.closeButton}`}
                        onClick={() => closeModal(modalName)}
                    >
                        &times;
                    </p>
                    <p className={`text-error ${error ? "visible" : "invisible"} ${styles.loginError}`}>{ error || "\u00a0" }</p>
                    <form onSubmit={(e) => {loginUser(e)}}>
                        <ErrorableFormField 
                            id="email"
                            label="Email"
                            value={email}
                            placeholder="Your email"
                            onChange={(e)=> setEmail(e.target.value)}
                            classNameWrapper={styles.inputWrapper}                        
                        />
                        <ErrorableFormField 
                            id="password"
                            label="Password"
                            type="password"
                            value={password}
                            placeholder="Password"
                            onChange={(e)=> setPassword(e.target.value)}
                            classNameWrapper={styles.inputWrapper}
                        />
                        <button type="submit" className={`button-primary cursor-pointer`}>Submit</button>
                        <p className={`text-success ${success ? "visible" : "invisible"}`}>Login successful!</p>
                    </form>
                    <div className={styles.promptRegister}>
                        <p>Need an account?</p>
                        <p
                            className={`text-secondary cursor-pointer`}
                            onClick={()=>{closeModal(modalName);openModal("register")}}
                            >
                                Register
                            </p>
                    </div>
                </div>
            </div>

        </ModalBase>
    );
}

export default LoginModal;