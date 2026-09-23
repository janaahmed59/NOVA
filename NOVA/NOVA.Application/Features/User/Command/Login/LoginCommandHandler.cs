using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Interfaces;
using MediatR;
using NOVA.Domain.Common.Results;
using NOVA.Application.Common.Errors;
using NOVA.Domain.Entities;

namespace NOVA.Application.Features.User.Command.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        public LoginCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, IJwtService jwtService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }
        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Implement the login logic here
            // For example, you can check the user's credentials against the database
            // and return a Result<Unit> indicating success or failure.
            // This is just a placeholder implementation.
            var user = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);                                             
            if(user == null)
            {
                return Result<LoginResponse>.Failure(NovaErrors.InvalidCredentials);
            }
            if(!user.IsActive)
            {
                return Result<LoginResponse>.Failure(NovaErrors.AccountDeactivated);
            }
            var passwordIsvalid = _passwordHasher.VerifyPassword(user, request.Password, user.PasswordHash);
            if (!passwordIsvalid)
            {
                return Result<LoginResponse>.Failure(NovaErrors.InvalidCredentials);
            }
            var accessToken = _jwtService.GenerateAccessToken(user);
            var result = new LoginResponse(user.FullName, user.Email, accessToken);
            return Result<LoginResponse>.Success(result);
        }
    }
}
