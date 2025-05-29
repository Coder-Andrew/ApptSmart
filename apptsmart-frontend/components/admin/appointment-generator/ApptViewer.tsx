import styles from './ApptViewer.module.css';
import { FaRegTrashAlt } from "react-icons/fa";

type ApptViewerProps = {
    appointments: AppointmentMap;
    handleDelete: (date: string, index: number) => void;
    handleUndo: (date: string, index: number) => void;
};

const ApptViewer = ({ appointments, handleDelete, handleUndo }: ApptViewerProps) => {
    const formatDate = (isoDate: string) => {
        const [year, month, day] = isoDate.split('-').map(Number);
        // Create date in local timezone to avoid UTC conversion issues
        const dateObj = new Date(year, month - 1, day);
        return dateObj.toLocaleDateString(undefined, {
            weekday: "short",
            month: "numeric",
            day: "numeric"
        });
    };

    const formatAppt = ({ start, end }: Appointment) => {
        // No need to do any timezone conversion here since the Date objects
        // already have the correct time information
        const startTime = start.toLocaleTimeString(undefined, {
            hour: "numeric",
            minute: "numeric"
        });
        const endTime = end.toLocaleTimeString(undefined, {
            hour: "numeric",
            minute: "numeric"
        });
        return `${startTime} - ${endTime}`;
    };

    if (Object.keys(appointments).length === 0) return null;

    const allDates = Object.keys(appointments)
    const numSlots = Math.max(...Object.values(appointments).map(list => list.length));

    // TODO: Add some more interaction besides just click to delete
    return (
        <div className={`${styles.tableWrapper}`}>
            <table className={`${styles.table}`}>
                <thead>
                    <tr>
                        {allDates.map(day => (
                            <th key={day} className={`bg-tertiary`}>{formatDate(day)}</th>
                        ))}
                    </tr>
                </thead>
                <tbody>
                    {Array.from({ length: numSlots }).map((_, i) => (
                        <tr key={i}>
                            {allDates.map(day => {
                                const appt = appointments[day][i];
                                return (
                                    <td 
                                        key={day + i}
                                        className={`cursor-pointer no-select ${appt.active ? styles.active : styles.disabled}`}
                                        onClick={() => appt.active ? handleDelete(day, i) : handleUndo(day, i)}
                                    >
                                        <FaRegTrashAlt className={`text-error ${styles.trash}`} />
                                        <span className={styles.apptText}>{formatAppt(appt)}</span>
                                    </td>
                                )
                            })}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default ApptViewer;