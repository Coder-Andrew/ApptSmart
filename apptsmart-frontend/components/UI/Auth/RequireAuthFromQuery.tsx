"use client"

import { useModal } from "@/providers/ModalProvider";
import { useUser } from "@/stores/UserContext";
import { useSearchParams } from "next/navigation";
import { useEffect, useRef } from "react";

const RequireAuthFromQuery = () => {
    const searchParams = useSearchParams();
    const { user, authReady, setRedirectPath } = useUser();
    const { openModal } = useModal();

    const redirect = searchParams.get("redirect");
    const hasHandledRedirect = useRef(false);

    useEffect(() => {
        if (redirect &&
            authReady &&
            !user &&
            !hasHandledRedirect.current
        ) {
            hasHandledRedirect.current = true;
            setRedirectPath(redirect);
            openModal("login");
        }
    }, [redirect, authReady, user, setRedirectPath, openModal]);
    
    return null;
}
 
export default RequireAuthFromQuery;