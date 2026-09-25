using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.User.Command.Login;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Entities;
namespace NOVA.Application.Features.User.Command.RefreshTokens
{
    public class RefreshTokenCommandHandler
     : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(
            IApplicationDbContext context,
            IRefreshTokenService refreshTokenService,
            IJwtService jwtService)
        {
            _context = context;
            _refreshTokenService = refreshTokenService;
            _jwtService = jwtService;
        }
        public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request ,CancellationToken cancellationToken)
        {
            var tokenHash = _refreshTokenService.HashToken(request.RefreshToken);

            var refreshToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(
                    rt => rt.TokenHash == tokenHash,
                    cancellationToken);

            if (refreshToken == null)
            {
                return Result<LoginResponse>.Failure(NovaErrors.TokenInvalid);
            }

            if (refreshToken.RevokedAt != null)
            {
                return Result<LoginResponse>.Failure(NovaErrors.TokenReused);
            }
            if (refreshToken.ExpiresAt <= DateTime.UtcNow)
            {
                return Result<LoginResponse>.Failure(
                    NovaErrors.TokenInvalid);
            }
            var user = refreshToken.User;

            if (!user.IsActive)
            {
                return Result<LoginResponse>.Failure(
                    NovaErrors.AccountDeactivated);
            }
            refreshToken.RevokedAt = DateTime.UtcNow;
            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _refreshTokenService.GenerateToken();

            var newRefreshTokenHash = _refreshTokenService.HashToken(newRefreshToken);

            var newRefreshTokenEntity = new RefreshToken
            {
                TokenHash = newRefreshTokenHash,
                UserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = _refreshTokenService.GetExpiration()
            };
            _context.RefreshTokens.Add(newRefreshTokenEntity);
            await _context.SaveChangesAsync(cancellationToken);
            var result = new LoginResponse(
                user.FullName,
                user.Email,
                newAccessToken,
                newRefreshToken);

            return Result<LoginResponse>.Success(result);
        }
    }
}
