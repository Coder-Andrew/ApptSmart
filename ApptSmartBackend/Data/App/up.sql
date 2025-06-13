CREATE TABLE [UserInfo] (
	[Id] UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
	[AspNetIdentityId] NVARCHAR(450) NOT NULL,
	[FirstName] NVARCHAR(150),
	[LastName] NVARCHAR(150)
);

CREATE TABLE [Appointments] (
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[StartTime] DATETIME NOT NULL,
	[EndTime] DATETIME NOT NULL,
);

CREATE TABLE [UserAppointments] (
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[UserInfoId] UNIQUEIDENTIFIER NOT NULL,
	[AppointmentId] INT NOT NULL,
	[BookedAt] DATETIME DEFAULT GETDATE(),
	CONSTRAINT [FK_UserAppointments_UserInfo] FOREIGN KEY ([UserInfoId]) REFERENCES UserInfo ([Id]),
	CONSTRAINT [FK_UserAppointments_Appointments] FOREIGN KEY ([AppointmentId]) REFERENCES Appointments ([Id]),
	CONSTRAINT [UQ_UserAppointments_AppointmentId] UNIQUE ([AppointmentId])
);