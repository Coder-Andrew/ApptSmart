import AppointmentItem, { AppointmentItemProps } from "../AppointmentItem/AppointmentItem";


export interface AppointmentContainerProps {
    header: string;
    items: Array<AppointmentItemProps>;
}

const AppointmentContainer: React.FC<AppointmentContainerProps> = ({items, header}) => {
    return (
        <>
            { items.length > 0 && (
                <>                
                    <h1>{header}</h1>
                    { items.map((appt, index) => (
                        <AppointmentItem key={appt.id} {...appt} />
                    ))}
                </>
            )}
        </>
    );
}



export default AppointmentContainer;