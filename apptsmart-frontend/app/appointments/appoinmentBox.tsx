"use client"

import AppointmentContainer from "@/components/UI/Appointments/AppointmentContainer/AppointmentContainer";
import AppointmentItem from "@/components/UI/Appointments/AppointmentItem/AppointmentItem";
import { Appointment, UserAppointmentProps } from "@/lib/types";
import { fetchBackend, getCsrftoken } from "@/utilities/helpers";
import { useEffect, useState } from "react";



const AppointmentBox = () => {
    const [ futureAppointments, setFutureApointments] = useState<UserAppointmentProps[]>([]);
    const [ pastAppointments, setPastAppointments ] = useState<UserAppointmentProps[]>([]);
    const [ upcomingAppointment, setUpcomingAppointment ] = useState<UserAppointmentProps[]>([]);

    useEffect(() => {
        getFutureAppointments();
        getPastAppointments();
    }, []);

    const getFutureAppointments = async () => {
        const response = await fetchBackend("/userAppointments/futureAppointments", {
            method: "GET",
            credentials: "include"
        });

        if (!response.ok) {
            return;
        }

        const jsonResponse: UserAppointmentProps[] = await response.json();
        const sortedAppointments = jsonResponse.sort((a, b) => {
            const dateA = new Date(a.appointment.startTime).getTime();
            const dateB = new Date(b.appointment.startTime).getTime();
            return dateA - dateB;
        })

        const nextAppointment = sortedAppointments[0];

        setFutureApointments(jsonResponse);
        setUpcomingAppointment([nextAppointment]);
    };

    const getPastAppointments = async () => {
        const response = await fetchBackend("/userAppointments/pastAppointments", {
            method: "GET"
        });

        if (!response.ok) {
            return;
        }
        const jsonResponse: UserAppointmentProps[] = await response.json();

        setPastAppointments(jsonResponse);
    }
    
    return (  
        <>
            <AppointmentContainer header="Upcoming Appointment" items={upcomingAppointment}/>
            <AppointmentContainer header="Future Appointments" items={futureAppointments}/>
            <AppointmentContainer header="Past Appointments" items={pastAppointments} />
            <div className="footer-spacer" />
        </>
    );
}
 
export default AppointmentBox;