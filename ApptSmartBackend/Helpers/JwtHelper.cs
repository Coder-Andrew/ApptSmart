using ApptSmartBackend.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApptSmartBackend.SettingsObjects;
using Microsoft.Extensions.Options;

namespace ApptSmartBackend.Helpers
{
    public class JwtHelper
    {
        private readonly JwtSettings _jwtSettings;
        public JwtHelper(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: credentials
                // issuer: _jwtSetting.Issuer       // CHANGE IN PRODUCTIONS
                // audience: _jwtSetting.Audience   // CHANGE IN PRODUCTIONS

            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
