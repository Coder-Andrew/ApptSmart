"use client"

import { Calendar } from "@/components/schedule-appointments/calendar";
import { CompanyInformation } from "@/lib/types";
import { fetchBackend } from "@/utilities/helpers";
import { useParams } from "next/navigation";
import { useEffect, useState } from "react";

const CompanySchedule = () => {
    const params = useParams();
    const companySlug = typeof params.companySlug === "string" ? params.companySlug : "";

    const [ pageError, setPageError ] = useState<string | null>(null);
    const [ isLoading, setIsLoading ] = useState(true);
    const [ companyInfo, setCompanyInfo ] = useState<CompanyInformation | null>(null);

    useEffect(() => {
        if (!companySlug) {
            setPageError("Not found");
            setIsLoading(false);
            return;
        };

        const getCompanyInfo = async () => {
            try {
                const res = await fetchBackend(`/company/${companySlug}`);
                if (!res.ok) throw new Error("Not found");
                const jsonRes: CompanyInformation = await res.json();
                setCompanyInfo(jsonRes);
            } catch {
                setPageError("Not found");
            } finally {
                setIsLoading(false);
            }
            
        }

        getCompanyInfo();

    }, [companySlug])

    if (isLoading) {
        return <div className="container"><p>Loading...</p></div>
    }

    if (pageError) {
        return <div className="container"><p className="text-error">{pageError}</p></div>
    }
    

    return (
        <div className="container">
            <p>{companyInfo?.companyName}</p>
            <p>{companyInfo?.companyDescription}</p>
            <Calendar companySlug={companySlug} />
            <div className="footer-spacer" />
        </div>
    );
}
 
export default CompanySchedule;