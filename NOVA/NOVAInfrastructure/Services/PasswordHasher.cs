using Microsoft.AspNetCore.Identity;
using NOVA.Application.Common.Interfaces;
using NOVA.Domain.Entities;

namespace NOVAInfrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly IPasswordHasher<User> _hasher;
        public PasswordHasher(IPasswordHasher<User> hasher)
        {
            _hasher = hasher;
        }
        public string HashPassword(User user, string password)
        {
            return _hasher.HashPassword(user, password);
        }
        public bool VerifyPassword(User user, string password, string hashedPassword)
        {
            var result = _hasher.VerifyHashedPassword(user, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
