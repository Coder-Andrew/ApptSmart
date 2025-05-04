import "./time-slot.css"

interface TimeSlotProps {
  time: string
  isTaken: boolean
}

export function TimeSlot({ time, isTaken }: TimeSlotProps) {
  return (
    <button className={`time-slot ${isTaken ? "taken" : "available"}`} disabled={isTaken}>
      {time}
    </button>
  )
}