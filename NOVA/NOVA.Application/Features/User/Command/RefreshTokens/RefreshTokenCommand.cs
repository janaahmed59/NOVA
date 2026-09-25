using MediatR;
using NOVA.Application.Features.User.Command.Login;
using NOVA.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace NOVA.Application.Features.User.Command.RefreshTokens
{
    public record RefreshTokenCommand(
    string RefreshToken
) : IRequest<Result<LoginResponse>>;
}
