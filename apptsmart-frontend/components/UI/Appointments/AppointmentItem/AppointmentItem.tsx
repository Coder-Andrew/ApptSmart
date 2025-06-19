"use client"
import { Appointment, UserAppointmentProps } from "@/lib/types";

const AppointmentItem = ({appointment, bookedAt, companyName}: UserAppointmentProps) => {
    const startTimeDate = new Date(appointment.startTime);
    const endTimeDate = new Date(appointment.endTime);
    //console.log(startTime, endTime);

    return (
        <div>
            <p>{ startTimeDate.toLocaleString() } - { endTimeDate.toLocaleString()} - {companyName}</p>
        </div>
    );
}
 
export default AppointmentItem;