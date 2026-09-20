using MediatR;
using NOVA.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace NOVA.Application.Features.User.Command.Register
{
    public record RegisterCommand(
    string FullName,
    string Email,
    string Password
) : IRequest<Result<Unit>>;
}
