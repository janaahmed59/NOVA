using Microsoft.Extensions.Options;
using NOVA.Application.Common.Interfaces;
using NOVAInfrastructure.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace NOVAInfrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly JwtSettings _settings;

        public RefreshTokenService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public string GenerateToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            return Convert.ToBase64String(randomBytes);
        }

        public string HashToken(string token)
        {
            var hash = SHA256.HashData(
                Encoding.UTF8.GetBytes(token));

            return Convert.ToHexString(hash);
        }

        public DateTime GetExpiration()
        {
            return DateTime.UtcNow.AddDays(
                _settings.RefreshTokenExpirationDays);
        }
    }
}
