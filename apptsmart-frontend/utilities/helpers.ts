import { Appointment, RawAppointment } from "@/lib/types"

export function toAppointment({id, startTime, endTime}: RawAppointment): Appointment {
    const start = new Date(startTime);
    const end = new Date(endTime);

    if (isNaN(start.getDate()) || isNaN(end.getTime())) {
        // TODO: Change later, maybe return null instead
        throw new Error("Invalid date string passed to toAppointment");
    }
    return {
        id: id,
        startTime: new Date(startTime),
        endTime: new Date(endTime)
    }
}