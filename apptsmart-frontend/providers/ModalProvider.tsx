"use client"

import { createContext, ReactNode, useContext, useState } from "react";

export type ModalType = 'register' | 'login';

interface ModalContextType {
    openModal: (modalType: ModalType) => void;
    closeModal: (modalType: ModalType) => void;
    isModalOpen: (modalType: ModalType) => boolean;
    currentOpenModals: ModalType[];
}

const ModalContext = createContext<ModalContextType | undefined>(undefined);

export const ModalProvider: React.FC<{ children: ReactNode}> = ({ children }) => {
    const [ openModals, setOpenModals ] = useState<ModalType[]>([]);

    const openModal = (modalType: ModalType) => {
        setOpenModals(prev => 
            prev.includes(modalType) ? prev : [...prev, modalType]
        );
    };

    const closeModal = (modalType?: ModalType) => {
        if (modalType) {
            setOpenModals(prev => prev.filter(modal => modal !== modalType));
        } else {
            setOpenModals([]);
        }
    };

    const isModalOpen = (modalType?: ModalType) => {
        if (modalType) {
            return openModals.includes(modalType);
        }
        return openModals.length > 0;
    }
    
    return ( 
        <ModalContext.Provider value={{
            openModal,
            closeModal,
            isModalOpen,
            currentOpenModals: openModals
        }}>
            { children }
        </ModalContext.Provider>
    );
}

export const useModal = () => {
    const context = useContext(ModalContext);
    if (context === undefined) throw new Error('useModal must be used within a ModalProvider');
    return context;
}