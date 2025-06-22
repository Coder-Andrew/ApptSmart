using System;
using System.Collections.Generic;

namespace ApptSmartBackend.Models.AppModels;

public partial class AvailableAppointment
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }
}
