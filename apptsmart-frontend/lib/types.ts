export interface RawAppointment {
    id: number,
    startTime: string,
    endTime: string
}
export interface Appointment {
    id: number,
    startTime: Date,
    endTime: Date
}
export interface UserAppointmentProps {
    bookedAt: Date,
    appointment: Appointment;
    companyName: string;
}
export interface CompanyInformation {
    companyName: string,
    companyDescription: string
}


export type AdminAppointment = { start: Date; end: Date, active: boolean }; // TODO: Could convert logic in appt creation to {appt: Appointment, active: boolean}
export type AppointmentMap = Record<string, AdminAppointment[]>;