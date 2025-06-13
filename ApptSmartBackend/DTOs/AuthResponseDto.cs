namespace ApptSmartBackend.DTOs
{
    public class AuthResponseDto
    {
        public string Jwt { get; set; } = string.Empty;
        public string CsrfToken { get; set; } = string.Empty;
    }
}
