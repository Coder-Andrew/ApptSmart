type Appointment = { start: Date; end: Date, active: boolean };
type AppointmentMap = Record<string, Appointment[]>;