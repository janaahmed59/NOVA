using System;
using System.Collections.Generic;
using System.Text;

namespace NOVA.Application.Common.Interfaces
{
    public interface IRefreshTokenService
    {
        string GenerateToken();

        string HashToken(string token);

        DateTime GetExpiration();
    }
}
