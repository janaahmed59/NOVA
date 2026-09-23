using NOVA.Domain.Entities;

namespace NOVA.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
    }
}
