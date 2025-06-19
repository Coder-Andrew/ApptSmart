using System;
using System.Collections.Generic;

namespace ApptSmartBackend.Models.AppModels;

public partial class UserInfo
{
    public Guid Id { get; set; }

    public string AspNetIdentityId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<UserAppointment> UserAppointments { get; set; } = new List<UserAppointment>();
}
