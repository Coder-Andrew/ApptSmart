using ApptSmartBackend.Helpers;
using ApptSmartBackend.Models;
using ApptSmartBackend.Services.Abstract;
using ApptSmartBackend.SettingsObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ApptSmartBackend.Services.Concrete
{
    /// <summary>
    /// Service responsible for mangaing refresh token, including token creation, storage, assignment, revocation, and validation.
    /// </summary>
    /// <remarks>
    /// This service uses <see cref="AuthDbContext"/> to persist tokens and <see cref="JwtHelper"/> to generate them.
    /// It may benefit from separation of concerns via a dedicated repository layer in future iterations.
    /// </remarks>
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AuthDbContext _authDbContext;
        private readonly JwtHelper _jwtHelper;
        private readonly JwtSettings _jwtSettings;
        public RefreshTokenService(AuthDbContext authDbContext, JwtHelper jwtHelper, IOptions<JwtSettings> options)
        {
            _authDbContext = authDbContext;
            _jwtHelper = jwtHelper;
            _jwtSettings = options.Value;
        }

        /// <summary>
        /// Generates a new refresh token for the specified user and stores it in the database.
        /// </summary>
        /// <param name="user">An authenticated user requesting a new refresh token.</param>
        /// <param name="fixedExpiry">Optional custome expiration time. Defaults to <c>RefreshExpiryMinutes</c>.</param>
        /// <returns>The newly created <see cref="RefreshToken"/>.</returns>
        public async Task<RefreshToken> CreateRefreshTokenAsync(AuthUser user, DateTime? fixedExpiry = null)
        {
            var token = _jwtHelper.GenerateUrlSafeToken(64);

            var refreshToken = new RefreshToken
            {
                Created = DateTime.UtcNow,
                Expires = fixedExpiry ?? DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshExpiryMinutes),
                Token = token,
                UserId = user.Id,
                IsRevoked = false,
            };

            await _authDbContext.RefreshTokens.AddAsync(refreshToken);
            await _authDbContext.SaveChangesAsync();

            return refreshToken;
        }

        /// <summary>
        /// Retrieves a valid (non-revoked, non-expired) refresh token and its associated user from the database.
        /// </summary>
        /// <param name="token">The refresh token's string to validate and lookup.</param>
        /// <returns>A <see cref="RefreshToken"/> including the associated user if valid; otherwise, <c>null</c>.</returns>
        public async Task<RefreshToken?> GetValidRefreshTokenAsync(string token)
        {
            return await _authDbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked && rt.Expires > DateTime.UtcNow);
        }

        /// <summary>
        /// Revokes a refresh token by setting its <c>IsRevoked</c> flag to true.
        /// </summary>
        /// <param name="token">The token string to revoke.</param>
        /// <remarks>
        /// No action is taken if the token is not found in the database.
        /// </remarks>
        public async Task RevokeRefreshTokenAsync(string token)
        {
            var refreshToken = await _authDbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);
            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                await _authDbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Stores a <see cref="RefreshToken"/> directly into the database.
        /// </summary>
        /// <param name="refreshToken">The <see cref="RefreshToken"/> to be added to the database.</param>
        /// <remarks>
        /// Typically used when tokens are generated outside this service.
        /// </remarks>
        public async Task SaveRefreshToken(RefreshToken refreshToken)
        {
            await _authDbContext.RefreshTokens.AddAsync(refreshToken);
            await _authDbContext.SaveChangesAsync();
        }
    }
}
