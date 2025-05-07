"use client"
import TimeSelect from "@/components/admin/appointment-generator/TimeSelect";
import ErrorableFormField from "@/components/UI/ErrorableFormField";
import { useEffect, useState } from "react";

type Appointment = { start: Date; end: Date };

const AdminPage = () => {
    // TODO: Setup for UTC capabilities
    // TODO: Move this logic into a component
    // TODO: Set up for 12-hour format instead of 24-hour
    const today = new Date();

    const [ startDate, setStartDate ] = useState('');
    const [ endDate, setEndDate ] = useState('');

    const [ startTimeHour, setStartTimeHour ] = useState(0);
    const [ startTimeMinute, setStartTimeMinute ] = useState(0)

    const [ endTimeHour, setEndTimeHour ] = useState(23);
    const [ endTimeMinute, setEndTimeMinute ] = useState(59);

    const [ apptLength, setApptLength ] = useState(60);
    const [ breakLength, setBreakLength ] = useState(0);

    const formatDate = (date: Date) => {
        const yyyy = date.getFullYear();
        const mm = String(date.getMonth() + 1).padStart(2, "0");
        const dd = String(date.getDate()).padStart(2,"0");
        return `${yyyy}-${mm}-${dd}`;
    }

    const generate = () => {
        console.log("generate")
        const start = new Date(startDate);
        start.setHours(startTimeHour, startTimeMinute)
        
        const end = new Date(endDate);
        end.setHours(endTimeHour, endTimeMinute);

        const apptDates: Record<string, Appointment[]> = {};

        let currentDay = new Date(start);
        while (currentDay <= end) {
            const dayKey = currentDay.toISOString().split('T')[0];
            apptDates[dayKey] = [];
            
            let dayStart = new Date(currentDay);
            dayStart.setHours(startTimeHour, startTimeMinute);

            let dayEnd = new Date(currentDay)
            dayEnd.setHours(endTimeHour, endTimeMinute);
            
            while (dayStart < dayEnd) {
                const apptStart = new Date(dayStart);
                dayStart.setMinutes(dayStart.getMinutes() + apptLength);
                const apptEnd = new Date(dayStart);;
                apptDates[dayKey].push({start: apptStart, end: apptEnd});
                dayStart.setMinutes(dayStart.getMinutes() + breakLength);                
            }
            currentDay.setDate(currentDay.getDate() + 1);
        }
        console.log(apptDates);
    }

    useEffect(() => {
        const now = new Date();
        const week = new Date();
        week.setDate(now.getDate() + 7);

        setStartDate(formatDate(now));
        setEndDate(formatDate(week));
    },[]);

    return (
        <>
            {/* MOVE OFF INTO COMPONENTS */}
            <p>Welcome to the admin page</p>
            <ErrorableFormField 
                id="startDate"
                label="Start Date"
                value={startDate}
                type="date"
                onChange={(e) => setStartDate(e.target.value)}
            />
            <ErrorableFormField 
                id="endDate"
                label="End Date"
                value={endDate}
                type="date"
                onChange={(e) => setEndDate(e.target.value)}
            />
            <TimeSelect
                label="Start Time"
                hour={startTimeHour}
                minute={startTimeMinute}
                onHourChange={setStartTimeHour}
                onMinuteChange={setStartTimeMinute}
            />
            <TimeSelect
                label="End Time"
                hour={endTimeHour}
                minute={endTimeMinute}
                onHourChange={setEndTimeHour}
                onMinuteChange={setEndTimeMinute}
            />
            <ErrorableFormField 
                id="apptLength"
                label="Appointment Length"
                value={apptLength}
                type="number"
                onChange={(e) => setApptLength(Number(e.target.value))}
            />
            <ErrorableFormField
                id="breakLength"
                label="Break Length"
                value={breakLength}
                type="number"
                onChange={(e) => setBreakLength(Number(e.target.value))}
            />
            <button 
                className={`button-primary cursor-pointer`}
                onClick={generate}
            >
                Generate Appointments
            </button>

            <div className="footer-spacer"></div>
        </>
    );
}
 
export default AdminPage;