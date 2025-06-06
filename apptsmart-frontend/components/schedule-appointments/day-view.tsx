'use client'

import { format } from "date-fns"
import { TimeSlot } from "./time-slot"
import styles from "./day-view.module.css"
import { useEffect, useState } from "react"
import { Appointment, RawAppointment } from "@/lib/types"
import { toAppointment } from "@/utilities/helpers"


interface DayViewProps {
  date: Date
}

export function DayView({ date }: DayViewProps) {
  const [dayError, setDayError] = useState<string>('');
  const [timeSlots, setTimeSlots] = useState<Appointment[]>([]);
  const [ selectedAppt, setSelectedAppt ] = useState<number|null>(null);

  console.log(selectedAppt);

  useEffect(() =>{
    getAvailableAppointments();
    setSelectedAppt(null);
  },[date]);

  const getAvailableAppointments = async () => {
    // TODO: Left off trying to get response from backend
    setDayError('');
    const params = new URLSearchParams({
      date: date.toISOString().split('T')[0]
    });
    const res = await fetch(`/api/backend/appointments/available?${params}`, {
      method: "GET"
    });

    if (!res.ok) {
      // TODO: Add better error handling
      setDayError('An error has occured, try again later.');
      return;
    }
    console.log(res)
    const data = await res.json();

    const dateData = data
      .map((a: RawAppointment) => toAppointment(a))
      .sort((a: Appointment, b: Appointment) => {
        return a.startTime.getTime() - b.startTime.getTime();
      });


    setTimeSlots(dateData);
  }


  return (
    <div className={styles.dayView}>
      {dayError && <p className="text-error">{ dayError }</p>}
      <h2 className={styles.dayTitle}>{format(date, "MMMM d, yyyy")}</h2>
      <div className={styles.timeSlotsGrid}>
        {timeSlots.length !== 0 ? timeSlots.map((slot) => (
          <TimeSlot 
            key={slot.id}
            id={slot.id}
            startTime={slot.startTime}
            endTime={slot.endTime}
            onSelect={() => setSelectedAppt(slot.id)}
            isSelected={selectedAppt === slot.id}
          />
        )) :
        (
          <div className="text-error">No available appointments for this date</div>
        )
        
      }
      </div>
      {selectedAppt && (
        <div className={styles.bookButton}>
          <button className={`button-primary cursor-pointer`}>Book</button>
        </div>
      )}
    </div>
  )
}
