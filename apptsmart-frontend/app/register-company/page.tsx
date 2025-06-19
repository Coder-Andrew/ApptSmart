"use client";
import ErrorableFormField from "@/components/UI/ErrorableFormField";
import { fetchBackend, slugify } from "@/utilities/helpers";
import { usePathname, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import styles from "./page.module.css";

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
            switch (res.status) {
                case 400:
                    setPostError("You cannot register more than one company at this time");
                    break;
                default:
                    setPostError("Something went wrong while creating your company.");
                    break;
            }
            setIsSubmitting(false);
            return;
        }

        setSuccess(`Successfully created ${companyName}! Redirecting...`);
        setTimeout(() => {
            router.push(`/c/${companySlug}/owner`);
        }, 3000);
    }

    return (
        <div className={`container ${styles.companyGroup}`}>
            <div className={styles.headerBox}>
                { success && <p className="text-success">{ success }</p>}
                { postError && <p className="text-error">{ postError }</p>}
                <h1>Want easy to manage appointment software for your company?</h1>
                <h2>Register below!</h2>
            </div>
            <div className={styles.inputBox}>
                <div className={styles.formHeaders}>
                    <h2>Company Registration</h2>
                    <h3>Get booking with AppSmart's professional scheduling solutions!</h3>
                </div>
                <div className={styles.formFields}>
                    <form onSubmit={handleSubmit}>
                        <ErrorableFormField
                            id={"companyName"}
                            type="text"
                            label="Company Name*"
                            placeholder="Enter your company name"
                            value={companyName}
                            error={companyNameError}
                            classNameWrapper={styles.formInputs}
                            onChange={(e) => setCompanyName(e.target.value)}
                            />
                        <div className={styles.formInputs}>
                            <label>Company Description</label>
                            <textarea 
                                onChange={(e) => setCompanyDescription(e.target.value)}
                                value={companyDescription}
                                placeholder="Tell us about your company and services..."
                            ></textarea>
                        </div>         
                        <p className="text-muted">Your shareable link will be: <code>{shareableLink}</code></p>
                        <button
                            type="submit"
                            className={`button-primary cursor-pointer ${styles.submitButton}`}
                            disabled={isSubmitting}
                            >
                                { isSubmitting ? "Creating..." : "Create Company!"}
                        </button>
                    </form>
                </div>
            </div>
            <div className="footer-spacer"/>
        </div>
    );
}
 
export default companyRegistration;