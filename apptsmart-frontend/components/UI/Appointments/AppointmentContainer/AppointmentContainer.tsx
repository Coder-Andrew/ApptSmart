import { UserAppointmentProps } from "@/lib/types";
import AppointmentItem from "../AppointmentItem/AppointmentItem";


export interface AppointmentContainerProps {
    header: string;
    items: Array<UserAppointmentProps>;
}

const AppointmentContainer = ({items, header}: AppointmentContainerProps) => {
    console.log(items);
    return (
        <>
            { items.length > 0 && (
                <>                
                    <h1>{header}</h1>
                    { items.map((userAppt, index) => (
                        <AppointmentItem key={index} {...userAppt} />
                    ))}
                </>
            )}
        </>
    );
}



export default AppointmentContainer;