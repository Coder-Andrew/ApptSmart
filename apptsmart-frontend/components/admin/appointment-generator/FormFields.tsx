import ErrorableFormField from "@/components/UI/ErrorableFormField";
import TimeSelect from "./TimeSelect";
import { useEffect, useState } from "react";
import styles from "./FormFields.module.css";

type FormFieldsProps = {
    onGenerate: (appointments: AppointmentMap) => void;
};

const FormFields = ({ onGenerate }: FormFieldsProps) => {
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [startTimeHour, setStartTimeHour] = useState(9);
    const [startTimeMinute, setStartTimeMinute] = useState(0);
    const [endTimeHour, setEndTimeHour] = useState(17);
    const [endTimeMinute, setEndTimeMinute] = useState(0);
    const [apptLength, setApptLength] = useState(60);
    const [breakLength, setBreakLength] = useState(0);

    const [ startDateError, setStartDateError ] = useState('');
    const [ endDateError, setEndDateError ] = useState('');
    const [ startTimeError, setStartTimeError ] = useState('');
    const [ endTimeError, setEndTimeError ] = useState('');
    const [ apptLengthError, setApptLengthError ] = useState('');
    const [ breakLengthError, setBreakLengthError ] = useState('');

    const createDateAtLocalTime = (dateStr: string, hour: number, minute: number) => {
        const [year, month, day] = dateStr.split('-').map(Number);
        // Create date in local time zone with the specified hour/minute
        return new Date(year, month - 1, day, hour, minute);
    };

    const validateInputs = () => {
        let valid = true;

        setStartDateError('');
        setEndDateError('');
        setStartTimeError('');
        setEndTimeError('');
        setApptLengthError('');
        setBreakLengthError('');

        if (!startDate) {
            setStartDateError('Start date is required');
            valid = false;
        }

        if (!endDate) {
            setEndDateError('End date is required');
            valid = false;
        }

        if (isNaN(startTimeHour) || isNaN(startTimeMinute)) {
            setStartTimeError('Start time must be a number');
            return false;
        }

        if (isNaN(endTimeHour) || isNaN(endTimeMinute)) {
            setEndTimeError('End time must be a number');
            return false;
        }

        // TODO: Check if dates are valid https://stackoverflow.com/questions/1353684/detecting-an-invalid-date-date-instance-in-javascript
        const start = new Date(startDate);
        const end = new Date(endDate);

        if (start > end) {
            setEndDateError('End date must be after start date');
            valid = false;
        }

        // TODO: Move today to startdate.setminutes(-1), to allow for same day appt creation
        const today = new Date(start);
        today.setHours(0,-1);

        if (start < today) {
            setStartDateError('Start date cannot be in the past');
            valid = false;
        }

        // TODO: Need to shortcircuit the NAN check above 
        // Adjust

        const startTime = createDateAtLocalTime("2000-01-01", startTimeHour, startTimeMinute);
        const endTime = createDateAtLocalTime("2000-01-01", endTimeHour, endTimeMinute);
        

        if (endTime <= startTime) {
            setEndTimeError('End time must be after start time');
            valid = false;
        }

        if (apptLength <= 0) {
            setApptLengthError('Appointment length must be greater than 0');
            valid = false;
        }

        if (breakLength < 0) {
            setBreakLengthError('Break length cannot be negative');
            valid = false;
        }

        return valid;
    }

    const generate = () => {
        if (!validateInputs()) return;
        const apptDates: Record<string, Appointment[]> = {};

        // Create start and end dates using local time
        const startDateObj = new Date(startDate);
        const endDateObj = new Date(endDate);
        
        // Iterate through each day
        let currentDate = new Date(startDateObj);
        while (currentDate <= endDateObj) {
            // Format as YYYY-MM-DD for the object key
            const dayKey = currentDate.toISOString().split('T')[0];
            apptDates[dayKey] = [];
            
            // Create the start and end time for this day
            const dayStart = createDateAtLocalTime(dayKey, startTimeHour, startTimeMinute);
            const dayEnd = createDateAtLocalTime(dayKey, endTimeHour, endTimeMinute);

            // Generate appointment slots for this day
            let slot = new Date(dayStart);
            while (slot < dayEnd) {
                const apptStart = new Date(slot);
                // Add appointment length
                slot = new Date(slot.getTime() + apptLength * 60_000);
                const apptEnd = new Date(slot);
                
                // Add this appointment to the day's list
                apptDates[dayKey].push({ start: apptStart, end: apptEnd, active: true });
                
                // Add break time
                slot = new Date(slot.getTime() + breakLength * 60_000);
            }
            
            // Move to next day
            currentDate.setDate(currentDate.getDate() + 1);
        }

        onGenerate(apptDates);
    };

    useEffect(() => {
        const localTomorrow = new Date();
        localTomorrow.setDate(localTomorrow.getDate() + 1);

        const localNextWeek = new Date(localTomorrow);
        localNextWeek.setDate(localTomorrow.getDate() + 6);

        const formatAsInputDate = (d: Date) =>
            `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;

        setStartDate(formatAsInputDate(localTomorrow));
        setEndDate(formatAsInputDate(localNextWeek));
    }, []);

    // TODO: Cleanup logic, could probably be iterated over to generate this...
    // Add time select error, or move time select to errorable form fields
    return (
        <div className={` ${styles.wrapper}`}>
            <div className={styles.inputs}>
                { [startTimeError, endTimeError].map((err, i) => (
                    err && <p key={i} className="text-error">{err}</p>
                ))}
                <ErrorableFormField id="startDate" label="Start Date" value={startDate} type="date" error={startDateError} classNameError="text-error" onChange={(e) => setStartDate(e.target.value)} />
                <ErrorableFormField id="endDate" label="End Date" value={endDate} type="date" error={endDateError} classNameError="text-error" onChange={(e) => setEndDate(e.target.value)} />
                <TimeSelect label="Start Time" hour={startTimeHour} minute={startTimeMinute} onHourChange={setStartTimeHour} onMinuteChange={setStartTimeMinute} />
                <TimeSelect label="End Time" hour={endTimeHour} minute={endTimeMinute} onHourChange={setEndTimeHour} onMinuteChange={setEndTimeMinute} />
                <ErrorableFormField id="apptLength" label="Appointment Length" error={apptLengthError} classNameError="text-error" value={apptLength} type="number" onChange={(e) => setApptLength(Number(e.target.value))} />
                <ErrorableFormField id="breakLength" label="Break Length" error={breakLengthError} classNameError="text-error" value={breakLength} type="number" onChange={(e) => setBreakLength(Number(e.target.value))} />
            </div>
            <div className={styles.submit}>
                <button className={`button-primary cursor-pointer ${styles.submit}`} onClick={generate}>Generate Appointments</button>
            </div>
        </div>
    );
};

export default FormFields;