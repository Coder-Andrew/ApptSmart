"use client"

export interface AppointmentItemProps {
    id: number;
    appointmentTime: string;
}


const AppointmentItem: React.FC<AppointmentItemProps> = ({id, appointmentTime: date}) => {
    const dateObj = new Date(date);
    // TODO: Add additional information, such as date, and time (converted to local time)
    const apptDate = dateObj.getDate();
    const apptTime = dateObj.getTime();

    return (
        <div>
            <p>{date}</p>
            <p>{apptDate}</p>
        </div>
    );
}
 
export default AppointmentItem;