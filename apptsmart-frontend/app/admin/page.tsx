"use client"
import ApptViewer from "@/components/admin/appointment-generator/ApptViewer";
import FormFields from "@/components/admin/appointment-generator/FormFields";
import TimeSelect from "@/components/admin/appointment-generator/TimeSelect";
import ErrorableFormField from "@/components/UI/ErrorableFormField";
import { useEffect, useState } from "react";



const AdminPage = () => {
    // TODO: Setup for UTC capabilities
    // TODO: Move this logic into a component
    // TODO: Set up for 12-hour format instead of 24-hour
    const [ appointments, setAppointments ] = useState<AppointmentMap>({});
    console.log(Object.keys(appointments));
    console.log(appointments)
    
    return (
        <>
            <p>Welcome to the admin page</p>
            <FormFields onGenerate={setAppointments} />
            <ApptViewer appointments={appointments}/>
            <div className="footer-spacer"></div>
        </>
    );
}
 
export default AdminPage;