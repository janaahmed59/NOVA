using NOVA.Application.Common.Interfaces;
using MediatR;
using NOVA.Domain.Entities;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.User.Command.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context;
        public LoginCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Unit>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Implement the login logic here
            // For example, you can check the user's credentials against the database
            // and return a Result<Unit> indicating success or failure.
            // This is just a placeholder implementation.
           
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
