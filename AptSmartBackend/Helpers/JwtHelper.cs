using AptSmartBackend.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AptSmartBackend.SettingsObjects;

namespace AptSmartBackend.Helpers
{
    public class JwtHelper
    {
        private readonly string _jwtSecret;
        private readonly int _jwtExpiryMinutes;
        public JwtHelper(IConfiguration configuration)
        {
            var jwtDetails = configuration.GetSection("Jwt").Get<JwtSettings>() ?? throw new KeyNotFoundException("Cannot find Jwt Settings");
            _jwtSecret = jwtDetails.Secret;
            _jwtExpiryMinutes = jwtDetails.ExpiryMinutes;
        }

        public string GenerateJwt(AuthUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtExpiryMinutes),
                signingCredentials: credentials
                // issuer: _jwtSetting.Issuer       // CHANGE IN PRODUCTIONS
                // audience: _jwtSetting.Audience   // CHANGE IN PRODUCTIONS

            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
