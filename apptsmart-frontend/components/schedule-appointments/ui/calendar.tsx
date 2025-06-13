"use client"

import { useEffect, useState } from "react"
import {
  format,
  startOfMonth,
  endOfMonth,
  eachDayOfInterval,
  isSameMonth,
  isSameDay,
  addMonths,
  subMonths,
} from "date-fns"
import "./calendar-ui.css"
import { fetchBackend } from "@/utilities/helpers"
import { Appointment } from "@/lib/types"

interface CalendarProps {
  mode: "single"
  selected?: Date
  onSelect?: (date: Date | undefined) => void
  className?: string
}

export function Calendar({ mode, selected, onSelect, className = "" }: CalendarProps) {
  const [currentMonth, setCurrentMonth] = useState(new Date())

  const [ availableDays, setAvailableDays ] = useState<Set<string>>();

  const monthStart = startOfMonth(currentMonth)
  const monthEnd = endOfMonth(currentMonth)
  const days = eachDayOfInterval({ start: monthStart, end: monthEnd })

  const today = new Date();
  today.setHours(0,0);
  today.setMinutes(-1);

  // Get day names for the header
  const weekDays = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"]

  // Calculate the day of the week for the first day of the month (0-6)
  const firstDayOfMonth = monthStart.getDay()

  // Create empty cells for days before the first day of the month
  const emptyDays = Array.from({ length: firstDayOfMonth }, (_, i) => i)

  const handlePreviousMonth = () => {
    setCurrentMonth(subMonths(currentMonth, 1))
  }

  const handleNextMonth = () => {
    setCurrentMonth(addMonths(currentMonth, 1))
  }

  const handleSelectDate = (date: Date) => {
    if (onSelect) {
      onSelect(date)
    }
  }

  const getAvailableDays = async () => {
    const res = await fetchBackend(`/appointments/available/${currentMonth.getMonth() + 1}`);

    if (!res.ok) {

      return;
    }
    const json: Array<string> = await res.json();
    setAvailableDays(new Set(json.map(d => new Date(d).getDate().toString())));
  }
  
  useEffect(() => {
    getAvailableDays();
  }, [currentMonth])

  return (
    <div className={`calendar-ui ${className}`}>
      <div className="calendar-header">
        <button className="month-nav" onClick={handlePreviousMonth}>
          &lt;
        </button>
        <h2 className="month-title">{format(currentMonth, "MMMM yyyy")}</h2>
        <button className="month-nav" onClick={handleNextMonth}>
          &gt;
        </button>
      </div>

      <div className="calendar-grid">
        {weekDays.map((day) => (
          <div key={day} className="weekday">
            {day}
          </div>
        ))}

        {emptyDays.map((_, index) => (
          <div key={`empty-${index}`} className="day empty"></div>
        ))}

        {days.map((day) => (
          <div className={`dayDiv no-select ${day < today ? "unavailable" : ""}`} key={day.toString()}>
            <button              
              className={`day ${!isSameMonth(day, currentMonth) ? "outside-month" : ""} ${selected && isSameDay(day, selected) ? "selected" : ""}`}
              onClick={() => day > today ? handleSelectDate(day) : ''}
            >
              {format(day, "d")}
              
            </button>
            <span>{availableDays?.has(format(day, "d")) ? '•' : '\u00a0'}</span>
          </div>
        ))}
      </div>
    </div>
  )
}