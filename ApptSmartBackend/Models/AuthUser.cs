using Microsoft.AspNetCore.Identity;

namespace ApptSmartBackend.Models
{
    public class AuthUser : IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
