using NOVA.Application.Common.Interfaces;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
namespace NOVA.Application.Features.User.Command.Register
{
    //
  //  Handler
  //├─ Check email
  //├─ Hash password
  //├─ Create User
  //└─ Save changes
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext db;
        private readonly IPasswordHasher passwordHasher;

        public RegisterCommandHandler(IApplicationDbContext db, IPasswordHasher passwordHasher)
        {
            this.db = db;
            this.passwordHasher = passwordHasher;
        }
        public async Task<Result<Unit>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if email already exists
            var existingUser = await db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (existingUser)
            {
                return Result<Unit>.Failure(NovaErrors.EmailTaken);
            }
            // create new user
            var user = new NOVA.Domain.Entities.User
            {
                FullName = request.FullName,
                Email = request.Email,
                IsActive = true,
                Role = Domain.Enums.GlobalRole.User,
                CreatedAt = DateTime.UtcNow
            };
            // Hash the password
            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

            // Add the user to the database
            db.Users.Add(user);
            await db.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
