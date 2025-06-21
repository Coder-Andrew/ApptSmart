'use client'

import { format } from "date-fns"
import { TimeSlot } from "./time-slot"
import styles from "./day-view.module.css"
import { useCallback, useEffect, useState } from "react"
import { Appointment, RawAppointment } from "@/lib/types"
import { fetchBackend, toAppointment } from "@/utilities/helpers"
import ROUTES from "@/lib/routes"
import { redirect } from "next/navigation"


interface DayViewProps {
  date: Date
  companySlug: string
}

export function DayView({ companySlug, date }: DayViewProps) {
  // TODO: Make it so only future events are posted... maybe add another method for the whole calandar to grey out an appt date with no appts
  // and make it so users can't click on past appointment times
  const [dayError, setDayError] = useState<string>('');
  const [ bookError, setBookError ] = useState<string>('');

  const [timeSlots, setTimeSlots] = useState<Appointment[]>([]);
  const [ selectedAppt, setSelectedAppt ] = useState<number|null>(null);

  const getAvailableAppointments = useCallback(async () => {
    setDayError('');
    // TODO: Need to figure out local/iso conversion.
    const params = new URLSearchParams({
      date: date.toLocaleString()
    });

    const res = await fetchBackend(`${companySlug}/appointments/available?${params}`, {
      method: "GET"
    });

    if (!res.ok) {
      // TODO: Add better error handling
      setDayError('An error has occured, try again later.');
      return;
    }
    const data = await res.json();

    const dateData = data
      .map((a: RawAppointment) => toAppointment(a))
      .sort((a: Appointment, b: Appointment) => {
        return a.startTime.getTime() - b.startTime.getTime();
      });


    setTimeSlots(dateData);
  }, [companySlug, date]);

  const bookAppointment = async () => {
    setBookError('');
    const res = await fetchBackend(`${companySlug}/appointments/book`, {
      method: "POST",
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        appointmentId: selectedAppt
      })
    })

    if (!res.ok) {
      setBookError('Booking failed... please try again later.');
      return;
    }

    // TODO: Add a notification at the top of the screen
    redirect(ROUTES.appointments);
  }

  useEffect(() =>{
    getAvailableAppointments();
    setSelectedAppt(null);
  },[date, getAvailableAppointments]);

  return (
    <div className={styles.dayView}>
      {dayError && <p className="text-error">{ dayError }</p>}
      {bookError && <p className="text-error">{ bookError }</p>}
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
          <button className={`button-primary cursor-pointer`} onClick={() => bookAppointment()}>Book</button>
        </div>
      )}
    </div>
  )
}
