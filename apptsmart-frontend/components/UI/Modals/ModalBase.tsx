import React from "react";
import styles from "./ModalBase.module.css";
import { ModalType, useModal } from "@/providers/ModalProvider";

type ModalBaseProps = {
    children: React.ReactNode;
    modalName: ModalType;
}

const ModalBase: React.FC<ModalBaseProps> = ({ children, modalName }) => {
    const { closeModal } = useModal();

    return (
        <div className={styles.modalOverlay} onClick={() => closeModal(modalName)}>
            <div onClick={(e) => e.stopPropagation()}>
                { children }
            </div>
        </div>
    );
}
 
export default ModalBase;