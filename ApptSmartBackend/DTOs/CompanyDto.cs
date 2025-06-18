using System.ComponentModel.DataAnnotations;

namespace ApptSmartBackend.DTOs
{
    public class CompanyDto
    {
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyDescription { get; set; } = string.Empty;
    }

    public class CreateCompanyDto
    {
        [Required]
        [MaxLength(150)]
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyDescription { get; set; } = string.Empty;
    }
}
