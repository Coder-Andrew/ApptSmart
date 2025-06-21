"use client"
import ApptViewer from "@/components/admin/appointment-generator/ApptViewer";
import FormFields from "@/components/admin/appointment-generator/FormFields";
import { AppointmentMap } from "@/lib/types"
import { fetchBackend } from "@/utilities/helpers";
import { useParams } from "next/navigation";
import { useState } from "react";

const OwnerPage = () => {
    // TODO: Need to add validation to ensure user is an admin, logic won't be here
    // TODO: Move off into a component, keep the admin page clean and composed of admin components
    const params = useParams();
    const [ appointments, setAppointments ] = useState<AppointmentMap>({});
    const [ postError, setPostError ] = useState<string>('');
    const [ postSuccess, setPostSuccess ] = useState<string>('');

    // useEffect(() => {

    // })

    const handleDelete = (day: string, index: number) => {
        setAppointments(prev => {
            const updated = { ...prev };
            updated[day] = [...updated[day]];
            updated[day][index] = {
                ...updated[day][index],
                active: false
            };
            return updated;
        })
    }

    const handleUndo = (day: string, index: number) => {
        setAppointments(prev => {
            const updated = { ...prev };
            updated[day] = [...updated[day]];
            updated[day][index] = {
                ...updated[day][index],
                active: true
            };
            return updated;
        })
    }

    // TODO: Add error handling and confirmation message
    const onPost = async () => {
        setPostError('');
        setPostSuccess('');
        const payload = formatApptsForBackend(appointments);
        if (payload.length <= 0) return;

        const res = await fetchBackend(`/companies/${params.companySlug}/owner/appointments`, {
            headers: {
                'Content-Type': 'application/json'
            },
            method: "POST",
            body: JSON.stringify(payload)
        });

        if (!res.ok) {
            // TODO: Add more specific errors -- unauthorized, server error, etc...
            setPostError('Error occured when creating appointments, please try again later.');
        } else {
            setPostSuccess(`Successfully created ${payload.length} appointments!`);
        }
    }

    const formatApptsForBackend = (appointments: AppointmentMap):{startTime: string, endTime: string}[] => {
        return Object.values(appointments)
            .flat()
            .filter(appt => appt.active)
            .map(appt => ({
                startTime: appt.start.toISOString(),
                endTime: appt.end.toISOString()
            }));
    }
    
    return (
        <>
            <div className={`container`}>
                { postError && (
                    <p className="text-error">{ postError }</p>
                )}
                { postSuccess && (
                    <p className="text-success">{ postSuccess }</p>
                )}
                <FormFields onGenerate={setAppointments} />
                <ApptViewer 
                    appointments={appointments}
                    handleDelete={handleDelete}
                    handleUndo={handleUndo}
                />
                { appointments && (<button 
                    className="button-primary cursor-pointer"
                    onClick={() => onPost()}
                >
                    Post Appointments
                </button>)}
            </div>
            <div className="footer-spacer"></div>
        </>
    );
}
 
export default OwnerPage;