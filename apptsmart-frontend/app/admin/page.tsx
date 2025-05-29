"use client"
import ApptViewer from "@/components/admin/appointment-generator/ApptViewer";
import FormFields from "@/components/admin/appointment-generator/FormFields";
import TimeSelect from "@/components/admin/appointment-generator/TimeSelect";
import ErrorableFormField from "@/components/UI/ErrorableFormField";
import { useEffect, useState } from "react";



const AdminPage = () => {
    // TODO: Need to add validation to ensure user is an admin, logic won't be here
    const [ appointments, setAppointments ] = useState<AppointmentMap>({});
    console.log(Object.keys(appointments));
    console.log(appointments)

    const handleDelete = (day: string, index: number) => {
        setAppointments(prev => {
            const updated = { ...prev };
            updated[day] = [...updated[day]];
            updated[day][index] = {
                ...updated[day][index],
                active: false
            };
            return updated;
        })
    }

    const handleUndo = (day: string, index: number) => {
        setAppointments(prev => {
            const updated = { ...prev };
            updated[day] = [...updated[day]];
            updated[day][index] = {
                ...updated[day][index],
                active: true
            };
            return updated;
        })
    }

    // TODO: Add error handling and confirmation message
    const onPost = async () => {
        const payLoad = formatApptsForBackend(appointments);
        console.log(JSON.stringify(payLoad));
        const res = await fetch("/api/appointments/create", {
            headers: {
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify(payLoad)
        });

        console.log(res);
    }

    const formatApptsForBackend = (appointments: AppointmentMap):{startTime: string, endTime: string}[] => {
        return Object.values(appointments)
            .flat()
            .filter(appt => appt.active)
            .map(appt => ({
                startTime: appt.start.toISOString(),
                endTime: appt.end.toISOString()
            }));
    }
    
    return (
        <>
            <div className={`container`}>
                <FormFields onGenerate={setAppointments} />
                <ApptViewer 
                    appointments={appointments}
                    handleDelete={handleDelete}
                    handleUndo={handleUndo}
                />
            </div>
            <button 
                className="button-primary cursor-pointer"
                onClick={() => onPost()}
            >
                Post Appointments
            </button>
            <div className="footer-spacer"></div>
        </>
    );
}
 
export default AdminPage;