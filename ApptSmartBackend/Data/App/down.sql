ALTER TABLE [UserAppointments] DROP CONSTRAINT [FK_UserAppointments_UserInfo];
ALTER TABLE [UserAppointments] DROP CONSTRAINT [FK_UserAppointments_Appointments];
ALTER TABLE [UserAppointments] DROP CONSTRAINT [UQ_UserAppointments_AppointmentId];

ALTER TABLE [Company] DROP CONSTRAINT [FK_Company_UserInfo];
ALTER TABLE [Appointments] DROP CONSTRAINT [FK_Appointments_Company];

DROP TABLE [Company];
DROP TABLE [UserAppointments];
DROP TABLE [UserInfo];
DROP TABLE [Appointments];