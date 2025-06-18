import { UserAppointmentProps } from "@/lib/types";
import AppointmentItem from "../AppointmentItem/AppointmentItem";


export interface AppointmentContainerProps {
    header: string;
    items: Array<UserAppointmentProps>;
}

const AppointmentContainer = ({items, header}: AppointmentContainerProps) => {
    return (
        <>
            { items.length > 0 && (
                <>                
                    <h1>{header}</h1>
                    { items.map((userAppt, index) => (
                        <AppointmentItem key={userAppt.appointment.id} {...userAppt} />
                    ))}
                </>
            )}
        </>
    );
}



export default AppointmentContainer;