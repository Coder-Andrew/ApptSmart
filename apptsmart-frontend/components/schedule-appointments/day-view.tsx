import { format } from "date-fns"
import { TimeSlot } from "./time-slot"
import "./day-view.css"

interface DayViewProps {
  date: Date
}

export function DayView({ date }: DayViewProps) {
  // This would typically come from your backend
  const takenSlots = ["10:00 AM", "2:00 PM", "4:30 PM"]

  const timeSlots = []
  for (let hour = 9; hour < 21; hour++) {
    for (let minute = 0; minute < 60; minute += 30) {
      const time = new Date(date)
      time.setHours(hour, minute)
      const formattedTime = format(time, "h:mm a")
      const isTaken = takenSlots.includes(formattedTime)
      timeSlots.push({ time: formattedTime, isTaken })
    }
  }

  return (
    <div className="day-view">
      <h2 className="day-title">{format(date, "MMMM d, yyyy")}</h2>
      <div className="time-slots-grid">
        {timeSlots.map((slot) => (
          <TimeSlot key={slot.time} time={slot.time} isTaken={slot.isTaken} />
        ))}
      </div>
    </div>
  )
}
