"use client"

import { useUser } from "@/stores/UserContext";
import { useModal } from "@/providers/ModalProvider";
import { useRouter } from "next/navigation";
import Link from "next/link";

const ProtectedLinkButton = ({href, children, className}: {href: string; children: React.ReactNode; className: string | undefined}) => {
    const { user, setRedirectPath } = useUser();
    const { openModal } = useModal();
    const router = useRouter();

    const handleClick = (e: React.MouseEvent<HTMLAnchorElement>) => {
        if (!user) {
            e.preventDefault();
            setRedirectPath(href);
            openModal("login");
        }
    }

    return <Link href={href} onClick={handleClick} className={className}>{children}</Link>
}
 
export default ProtectedLinkButton;