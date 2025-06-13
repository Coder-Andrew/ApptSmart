using System;
using System.Collections.Generic;

namespace ApptSmartBackend.Models.AppModels;

public partial class UserAppointment
{
    public int Id { get; set; }

    public Guid UserInfoId { get; set; }

    public int AppointmentId { get; set; }

    public DateTime? BookedAt { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual UserInfo UserInfo { get; set; } = null!;
}
