import ErrorableFormField from "@/components/UI/ErrorableFormField";
import TimeSelect from "./TimeSelect";
import { useEffect, useState } from "react";

type FormFieldsProps = {
    onGenerate: (appointments: Record<string, Appointment[]>) => void;
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

    const createDateAtLocalTime = (dateStr: string, hour: number, minute: number) => {
        const [year, month, day] = dateStr.split('-').map(Number);
        // Create date in local time zone with the specified hour/minute
        return new Date(year, month - 1, day, hour, minute);
    };

    const generate = () => {
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
                apptDates[dayKey].push({ start: apptStart, end: apptEnd });
                
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

    return (
        <div>
            <ErrorableFormField id="startDate" label="Start Date" value={startDate} type="date" onChange={(e) => setStartDate(e.target.value)} />
            <ErrorableFormField id="endDate" label="End Date" value={endDate} type="date" onChange={(e) => setEndDate(e.target.value)} />
            <TimeSelect label="Start Time" hour={startTimeHour} minute={startTimeMinute} onHourChange={setStartTimeHour} onMinuteChange={setStartTimeMinute} />
            <TimeSelect label="End Time" hour={endTimeHour} minute={endTimeMinute} onHourChange={setEndTimeHour} onMinuteChange={setEndTimeMinute} />
            <ErrorableFormField id="apptLength" label="Appointment Length" value={apptLength} type="number" onChange={(e) => setApptLength(Number(e.target.value))} />
            <ErrorableFormField id="breakLength" label="Break Length" value={breakLength} type="number" onChange={(e) => setBreakLength(Number(e.target.value))} />
            <button className="button-primary cursor-pointer" onClick={generate}>Generate Appointments</button>
        </div>
    );
};

export default FormFields;