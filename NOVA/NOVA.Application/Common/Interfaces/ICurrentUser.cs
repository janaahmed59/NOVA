using NOVA.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace NOVA.Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        int? Id { get; }
        string? Email { get; }    
        GlobalRole? Role { get; }
        bool IsAuthenticated { get; }

    }
}
