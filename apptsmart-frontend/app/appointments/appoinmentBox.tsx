"use client"

import AppointmentContainer from "@/components/Appointments/AppointmentContainer/AppointmentContainer";
import AppointmentItem, { AppointmentItemProps } from "@/components/Appointments/AppointmentItem/AppointmentItem";
import { useEffect, useState } from "react";


const AppointmentBox = () => {
    const [ futureAppointments, setFutureApointments] = useState<AppointmentItemProps[]>([]);
    const [ pastAppointments, setPastAppointments ] = useState<AppointmentItemProps[]>([]);
    const [ upcomingAppointment, setUpcomingAppointment ] = useState<AppointmentItemProps[]>([]);

    useEffect(() => {
        GetFutureAppointments();
    }, []);

    const GetFutureAppointments = async () => {
        const response = await fetch("/api/appointments/futureAppointments", {
            method: "GET"
        });

        if (!response.ok) {
            console.log(response);
            return;
        }

        const jsonResponse: AppointmentItemProps[] = await response.json();
        console.log(jsonResponse);

        setFutureApointments(jsonResponse);
    };
    
    return (  
        <>
            {/* <AppointmentContainer header="Upcoming Appointment"/> */}
            <AppointmentContainer header="Future Appointments" items={futureAppointments}/>
            {/* <AppointmentContainer header="Past Appointments" items={} /> */}
        </>
    );
}
 
export default AppointmentBox;