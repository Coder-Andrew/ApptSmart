using System;
using System.Collections.Generic;

namespace ApptSmartBackend.Models.AppModels;

public partial class Company
{
    public int Id { get; set; }

    public Guid OwnerId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string CompanySlug { get; set; } = null!;

    public string? CompanyDescription { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual UserInfo Owner { get; set; } = null!;
}
