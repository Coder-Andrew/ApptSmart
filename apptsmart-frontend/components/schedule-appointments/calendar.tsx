"use client"

import { useState } from "react"
import { Calendar as CalendarComponent } from "./ui/calendar"
import { DayView } from "./day-view"
import "./calendar.css"

export function Calendar() {
  const [selectedDate, setSelectedDate] = useState<Date | undefined>(new Date())

  return (
    <div className="calendar-container">
      <div className="calendar-wrapper">
        <CalendarComponent mode="single" selected={selectedDate} onSelect={setSelectedDate} className="calendar" />
      </div>
      <div className="day-view-container">{selectedDate && <DayView date={selectedDate} />}</div>
    </div>
  )
}
