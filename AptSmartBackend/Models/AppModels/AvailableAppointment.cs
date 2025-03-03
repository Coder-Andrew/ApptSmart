using System;
using System.Collections.Generic;

namespace AptSmartBackend.Models.AppModels;

public partial class AvailableAppointment
{
    public int Id { get; set; }

    public DateTime Time { get; set; }

    public DateOnly Date { get; set; }
}
