"use client"

import AppointmentContainer from "@/components/UI/Appointments/AppointmentContainer/AppointmentContainer";
import AppointmentItem, { AppointmentItemProps } from "@/components/UI/Appointments/AppointmentItem/AppointmentItem";
import { useEffect, useState } from "react";


const AppointmentBox = () => {
    const [ futureAppointments, setFutureApointments] = useState<AppointmentItemProps[]>([]);
    const [ pastAppointments, setPastAppointments ] = useState<AppointmentItemProps[]>([]);
    const [ upcomingAppointment, setUpcomingAppointment ] = useState<AppointmentItemProps[]>([]);

    useEffect(() => {
        getFutureAppointments();
        getPastAppointments();
    }, []);

    const getFutureAppointments = async () => {
        const response = await fetch("/api/appointments/futureAppointments", {
            method: "GET"
        });

        if (!response.ok) {
            console.log(response);
            return;
        }

        const jsonResponse: AppointmentItemProps[] = await response.json();
        
        const sortedAppointments = jsonResponse.sort((a, b) => {
            const dateA = new Date(a.appointmentTime).getTime();
            const dateB = new Date(b.appointmentTime).getTime();
            return dateA - dateB;
        })

        const nextAppointment = sortedAppointments[0];

        setFutureApointments(jsonResponse);
        setUpcomingAppointment([nextAppointment]);
    };

    const getPastAppointments = async () => {
        const response = await fetch("/api/appointments/pastAppointments", {
            method: "GET"
        });

        if (!response.ok) {
            console.log(response);
            return;
        }

        const jsonResponse: AppointmentItemProps[] = await response.json();
        console.log(jsonResponse);

        setPastAppointments(jsonResponse);
    }
    
    return (  
        <>
            <AppointmentContainer header="Upcoming Appointment" items={upcomingAppointment}/>
            <AppointmentContainer header="Future Appointments" items={futureAppointments}/>
            <AppointmentContainer header="Past Appointments" items={pastAppointments} />
        </>
    );
}
 
export default AppointmentBox;