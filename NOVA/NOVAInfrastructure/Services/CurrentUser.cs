using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using NOVA.Application.Common.Interfaces;
using NOVA.Domain.Enums;

namespace NOVA.Infrastructure.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? Id
    {
        get
        {
            var userId = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirstValue(JwtRegisteredClaimNames.Sub);

            return int.TryParse(userId, out var id)
                ? id
                : null;
        }
    }

    public string? Email =>
        _httpContextAccessor
            .HttpContext?
            .User
            .FindFirstValue(JwtRegisteredClaimNames.Email);

    public GlobalRole? Role
    {
        get
        {
            var role = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirstValue(ClaimTypes.Role);

            return Enum.TryParse<GlobalRole>(role, out var result)
                ? result
                : null;
        }
    }

    public bool IsAuthenticated =>
        _httpContextAccessor
            .HttpContext?
            .User
            .Identity?
            .IsAuthenticated
            ?? false;
}