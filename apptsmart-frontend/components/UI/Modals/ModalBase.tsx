import React from "react";
import styles from "./ModalBase.module.css";
import { ModalType, useModal } from "@/providers/ModalProvider";

type ModalBaseProps = {
    children: React.ReactNode;
    modalName: ModalType;
    handleClose?: (modalName: ModalType) => void;
}

const ModalBase: React.FC<ModalBaseProps> = ({ children, modalName,  handleClose}) => {
    const { closeModal } = useModal();
    if (typeof handleClose === "undefined") {
        handleClose = closeModal;
    }

    /// LEFT OFF TRYING TO MAKE SURE CLOSE MODAL CAN EITHER BE PASSED IN OR DEFAULTS TO ORIGINAL CLOSE MODAL

    return (
        <div className={styles.modalOverlay} onClick={() => handleClose(modalName)}>
            <div onClick={(e) => e.stopPropagation()}>
                { children }
            </div>
        </div>
    );
}
 
export default ModalBase;