using ApptSmartBackend.Models.AppModels;
using ApptSmartBackend.Helpers;

namespace ApptSmartBackend.Utilities
{
    public class SeedCompany
    {
        public string CompanyName { get; set; } = string.Empty;
        public string CompanySlug { get; set; } = string.Empty;
        public string CompanyDescription { get; set; } = string.Empty;
        public SeedUserInfo Owner { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
        public SeedCompany(string companyName, string companyDescription, SeedUserInfo owner, List<Appointment> appointments)
        {
            CompanyName = companyName;
            CompanySlug = SlugHelper.Slugify(companyName);
            CompanyDescription = companyDescription;
            Owner = owner;
            Appointments = appointments;
        }
    }
}
