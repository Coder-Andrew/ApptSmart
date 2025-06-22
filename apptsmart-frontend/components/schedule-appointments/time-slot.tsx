import "./time-slot.css"

interface TimeSlotProps {
  id: number
  startTime: Date
  endTime: Date
  onSelect: () => void
  isSelected: boolean
}

export function TimeSlot({id, startTime, endTime, onSelect, isSelected }: TimeSlotProps ) {
  return (
    <button
      className={`time-slot ${isSelected ? 'selected' : ''}`}
      onClick={onSelect}
      data-id={id}
    >
      {`${startTime.toLocaleTimeString()} - ${endTime.toLocaleTimeString()}`}
    </button>
  )
}