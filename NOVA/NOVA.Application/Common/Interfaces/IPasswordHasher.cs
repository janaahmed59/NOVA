using NOVA.Domain.Entities;
namespace NOVA.Application.Common.Interfaces
{
    // Interface for password hashing and verification
    public interface IPasswordHasher
    {
        string HashPassword(User user ,string password);
        bool VerifyPassword(User user, string password, string hashedPassword);
    }
}
