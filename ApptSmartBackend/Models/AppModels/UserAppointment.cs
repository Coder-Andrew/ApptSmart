using System;
using System.Collections.Generic;

namespace ApptSmartBackend.Models.AppModels;

public partial class UserAppointment
{
    public int Id { get; set; }

    public Guid UserInfoId { get; set; }

    public DateTime DateTime { get; set; }

    public virtual UserInfo UserInfo { get; set; } = null!;
}
