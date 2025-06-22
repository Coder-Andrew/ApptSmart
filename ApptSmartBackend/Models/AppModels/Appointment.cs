using System;
using System.Collections.Generic;

namespace ApptSmartBackend.Models.AppModels;

public partial class Appointment
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual UserAppointment? UserAppointment { get; set; }
}
