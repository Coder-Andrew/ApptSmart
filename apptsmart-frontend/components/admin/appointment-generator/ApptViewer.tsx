import styles from './ApptView.module.css';

type ApptViewerProps = {
    appointments: Record<string, Appointment[]>;
};

const ApptViewer = ({ appointments }: ApptViewerProps) => {
    const formatDate = (isoDate: string) => {
        const [year, month, day] = isoDate.split('-').map(Number);
        // Create date in local timezone to avoid UTC conversion issues
        const dateObj = new Date(year, month - 1, day);
        return dateObj.toLocaleDateString(undefined, {
            weekday: "short",
            month: "short",
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

    const numSlots = Math.max(...Object.values(appointments).map(list => list.length));

    return (
        <table className={styles.table}>
            <thead>
                <tr>
                    {Object.keys(appointments).map(date => (
                        <th key={date}>{formatDate(date)}</th>
                    ))}
                </tr>
            </thead>
            <tbody>
                {Array.from({ length: numSlots }).map((_, i) => (
                    <tr key={i}>
                        {Object.keys(appointments).map(day => (
                            <td key={day + i}>
                                {appointments[day][i] ? formatAppt(appointments[day][i]) : ''}
                            </td>
                        ))}
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

export default ApptViewer;