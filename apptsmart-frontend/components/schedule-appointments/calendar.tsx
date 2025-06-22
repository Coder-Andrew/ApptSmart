"use client"

import { useState } from "react"
import { Calendar as CalendarComponent } from "./ui/calendar"
import { DayView } from "./day-view"
import "./calendar.css"

interface calandarProps {
  companySlug: string
}

export function Calendar({companySlug}: calandarProps) {
  const [selectedDate, setSelectedDate] = useState<Date | undefined>(new Date())

  return (
    <div className="calendar-container">
      <div className="calendar-wrapper">
        <CalendarComponent companySlug={companySlug} mode="single" selected={selectedDate} onSelect={setSelectedDate} className="calendar" />
      </div>
      <div className="day-view-container">{selectedDate && <DayView companySlug={companySlug} date={selectedDate} />}</div>
    </div>
  )
}
