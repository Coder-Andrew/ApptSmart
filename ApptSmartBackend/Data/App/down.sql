ALTER TABLE [UserAppointments] DROP CONSTRAINT [FK_UserAppointments_UserInfo];
ALTER TABLE [UserAppointments] DROP CONSTRAINT [FK_UserAppointments_Appointments];
ALTER TABLE [UserAppointments] DROP CONSTRAINT [UQ_UserAppointments_AppointmentId];

DROP TABLE [UserAppointments];
DROP TABLE [UserInfo];
DROP TABLE [Appointments];