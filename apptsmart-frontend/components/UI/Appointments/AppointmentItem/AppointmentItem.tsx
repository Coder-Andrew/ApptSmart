"use client"
import { Appointment } from "@/lib/types";

const AppointmentItem: React.FC<Appointment> = ({startTime, endTime}) => {
    const startTimeDate = new Date(startTime);
    const endTimeDate = new Date(endTime);
    //console.log(startTime, endTime);

    return (
        <div>
            <p>{ startTimeDate.toLocaleString() } - { endTimeDate.toLocaleString() }</p>
        </div>
    );
}
 
export default AppointmentItem;