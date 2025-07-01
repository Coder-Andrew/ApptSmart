using ApptSmartBackend.Models;

namespace ApptSmartBackend.DTOs
{
    public class AuthResponseDto
    {
        public string Jwt { get; set; } = string.Empty;
        public string CsrfToken { get; set; } = string.Empty;
        public RefreshToken RefreshToken { get; set; } = new();
    }
}
