"use client";
import ErrorableFormField from "@/components/UI/ErrorableFormField";
import { fetchBackend, slugify } from "@/utilities/helpers";
import { usePathname, useRouter } from "next/navigation";
import { useEffect, useState } from "react";

const companyRegistration = () => {
    const [ companyName, setCompanyName ] = useState("");
    const [ companyNameError, setCompanyNameError ] = useState("");
    const [ companyDescription, setCompanyDescription ] = useState("");
    const [ isSubmitting, setIsSubmitting ] = useState(false);
    const [ hostname, setHostName ] = useState("");
    const [ postError, setPostError ] = useState("");
    const [ success, setSuccess ] = useState("");
    const router = useRouter();

    const companySlug = slugify(companyName);
    const shareableLink = companySlug && hostname
        ? `${hostname}/c/${companySlug}/schedule`
        : "";

    useEffect(() => {
        if (typeof window !== "undefined") setHostName(window.location.origin);
    }, []);

    const validate = (): boolean => {
        let isValid = true;        
        setCompanyNameError("");

        if (!companyName.trim()) {
            setCompanyNameError("Must specify a company name!");
            isValid = false;
        }

        return isValid;
    }

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setSuccess("");
        setPostError("");

        if (!validate()) return;

        setIsSubmitting(true);
        const res = await fetchBackend("/company/create", {
            method: "POST",
            body: JSON.stringify({
                companyName,
                companyDescription
            }),
            headers: {
                "Content-Type": "application/json"
            }
        });
        if (!res.ok) {
            const errors = await res.json();
            if (errors?.companyName) setCompanyNameError(errors.companyName);
            else setPostError("Something went wrong while creating your company.");
            setIsSubmitting(false);
            return;
        }

        setSuccess(`Successfully created ${companyName}! Redirecting...`);
        setTimeout(() => {
            router.push(`/c/${companySlug}/owner`);
        }, 3000);
    }

    return (
        <div>
            <h1>Want easy to manage appointment software for your company?</h1>
            <h2>Register below!</h2>
            { success && <p className="text-success">{ success }</p>}
            <p>Your shareable link will be: <code>{shareableLink}</code></p>
            <form onSubmit={handleSubmit}>
                <ErrorableFormField
                    id={"companyName"}
                    type="text"
                    label="Company Name"
                    value={companyName}
                    error={companyNameError}
                    onChange={(e) => setCompanyName(e.target.value)}
                />
                <label>Company Description</label>            
                <textarea onChange={(e) => setCompanyDescription(e.target.value)} value={companyDescription}></textarea>
                <button
                    type="submit"
                    disabled={isSubmitting}
                >
                        { isSubmitting ? "Creating..." : "Create Company!"}
                </button>

            </form>
            <div className="footer-spacer"/>
        </div>
    );
}
 
export default companyRegistration;