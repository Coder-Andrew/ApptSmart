import React, { useEffect } from "react";
import styles from "./ModalBase.module.css";
import { ModalType, useModal } from "@/providers/ModalProvider";

type ModalBaseProps = {
    children: React.ReactNode;
    modalName: ModalType;
    handleClose?: (modalName: ModalType) => void;
}

const ModalBase: React.FC<ModalBaseProps> = ({ children, modalName,  handleClose}) => {
    const { closeModal, isModalOpen } = useModal();
    const open = isModalOpen(modalName);

    if (typeof handleClose === "undefined") {
        handleClose = closeModal;
    }

    useEffect(() => {
        if (open) {
            document.body.style.overflow = "hidden";
        } else {
            document.body.style.overflow = "";
        }

        return () => {
            document.body.style.overflow = "";
        };
    }, [open]);


    if (!open) return null;

    return (
        <div className={styles.modalOverlay} onClick={() => handleClose(modalName)}>
            <div onClick={(e) => e.stopPropagation()}>
                { children }
            </div>
        </div>
    );
}
 
export default ModalBase;