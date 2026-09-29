using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Cart.Commands.ClearCart
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public ClearCartCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Result<Unit>> Handle(ClearCartCommand request, CancellationToken ct)
        {
            var userId = _currentUser.Id;
            if (userId is null)
            {
                return Result<Unit>.Failure(NovaErrors.Unauthenticated);
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId.Value, ct);

            if (cart is null || cart.CartItems is null || !cart.CartItems.Any())
            {
                return Result<Unit>.Success(Unit.Value);
            }

            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync(ct);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}