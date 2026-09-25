using System;
using System.Collections.Generic;
using System.Text;

namespace NOVA.Application.Features.User.Command.Login
{
    public record LoginResponse
    (
         string FullName,
         string Email,
         string AccessToken,
         string RefreshToken);
}
