ALTER TABLE [UserAppointments] DROP CONSTRAINT [Fk UserAppointments UserInfoId];

DROP TABLE [UserAppointments];
DROP TABLE [UserInfo];
DROP TABLE [AvailableAppointments];