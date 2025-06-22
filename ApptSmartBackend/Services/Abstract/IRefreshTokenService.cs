using ApptSmartBackend.Models;

namespace ApptSmartBackend.Services.Abstract
{
    public interface IRefreshTokenService
    {
        /// <summary>
        /// Generates a new refresh token for the specified user and stores it in the database.
        /// </summary>
        /// <param name="user">An authenticated user requesting a new refresh token.</param>
        /// <param name="fixedExpiry">Optional custome expiration time. Defaults to <c>RefreshExpiryMinutes</c>.</param>
        /// <returns>The newly created <see cref="RefreshToken"/>.</returns>
        Task<RefreshToken> CreateRefreshTokenAsync(AuthUser user, DateTime? fixedExpiry = null);
        /// <summary>
        /// Revokes a refresh token by setting its <c>IsRevoked</c> flag to true.
        /// </summary>
        /// <param name="token">The token string to revoke.</param>
        /// <remarks>
        /// No action is taken if the token is not found in the database.
        /// </remarks>
        Task RevokeRefreshTokenAsync(string token);
        /// <summary>
        /// Stores a <see cref="RefreshToken"/> directly into the database.
        /// </summary>
        /// <param name="refreshToken">The <see cref="RefreshToken"/> to be added to the database.</param>
        /// <remarks>
        /// Typically used when tokens are generated outside this service.
        /// </remarks>
        Task SaveRefreshToken(RefreshToken refreshToken);
        /// <summary>
        /// Retrieves a valid (non-revoked, non-expired) refresh token and its associated user from the database.
        /// </summary>
        /// <param name="token">The refresh token's string to validate and lookup.</param>
        /// <returns>A <see cref="RefreshToken"/> including the associated user if valid; otherwise, <c>null</c>.</returns>
        Task<RefreshToken?> GetValidRefreshTokenAsync(string token);
    }
}
