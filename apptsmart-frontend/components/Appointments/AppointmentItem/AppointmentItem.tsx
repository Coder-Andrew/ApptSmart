"use client"

export interface AppointmentItemProps {
    id: number;
    appointmentTime: string;
}


const AppointmentItem: React.FC<AppointmentItemProps> = ({id, appointmentTime: date}) => {
    return (
        <div>
            <p>{date}</p>
        </div>
    );
}
 
export default AppointmentItem;