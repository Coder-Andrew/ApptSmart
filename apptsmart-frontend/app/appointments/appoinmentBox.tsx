"use client"

import AppointmentContainer from "@/components/UI/Appointments/AppointmentContainer/AppointmentContainer";
import AppointmentItem from "@/components/UI/Appointments/AppointmentItem/AppointmentItem";
import { Appointment, UserAppointmentProps } from "@/lib/types";
import { useEffect, useState } from "react";



const AppointmentBox = () => {
    const [ futureAppointments, setFutureApointments] = useState<Appointment[]>([]);
    const [ pastAppointments, setPastAppointments ] = useState<Appointment[]>([]);
    const [ upcomingAppointment, setUpcomingAppointment ] = useState<Appointment[]>([]);

    useEffect(() => {
        getFutureAppointments();
        getPastAppointments();
    }, []);

    const getFutureAppointments = async () => {
        const response = await fetch("/api/backend/appointments/futureAppointments", {
            method: "GET"
        });

        if (!response.ok) {
            console.log(response);
            return;
        }

        const jsonResponse: UserAppointmentProps[] = await response.json();
        console.log(jsonResponse);
        const sortedAppointments = jsonResponse.sort((a, b) => {
            const dateA = new Date(a.appointment.startTime).getTime();
            const dateB = new Date(b.appointment.startTime).getTime();
            return dateA - dateB;
        })

        const nextAppointment = sortedAppointments[0];

        setFutureApointments(jsonResponse.map((ua)=> ua.appointment));
        setUpcomingAppointment([nextAppointment.appointment]);
    };

    const getPastAppointments = async () => {
        const response = await fetch("/api/backend/appointments/pastAppointments", {
            method: "GET"
        });

        if (!response.ok) {
            console.log(response);
            return;
        }

        const jsonResponse: UserAppointmentProps[] = await response.json();
        //console.log(jsonResponse);

        setPastAppointments(jsonResponse.map((ua)=> ua.appointment));
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