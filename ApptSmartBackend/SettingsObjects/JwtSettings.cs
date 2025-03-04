using System.ComponentModel.DataAnnotations;

namespace ApptSmartBackend.SettingsObjects
{
    public class JwtSettings
    {
        [Required]
        [MinLength(50)]
        public string Secret { get; set; } = string.Empty;
        [Required]
        public int ExpiryMinutes { get; set; }
        //public string Issuer { get; set; }
        //public string Audience { get; set; }
        //public string RefreshTokenSecret { get; set; }
        //public int RefreshTokenExpiryDays { get; set; }
    }

}
