namespace AptSmartBackend
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public int ExpiryMinutes { get; set; }
        //public string Issuer { get; set; }
        //public string Audience { get; set; }
        //public string RefreshTokenSecret { get; set; }
        //public int RefreshTokenExpiryDays { get; set; }
    }
}
