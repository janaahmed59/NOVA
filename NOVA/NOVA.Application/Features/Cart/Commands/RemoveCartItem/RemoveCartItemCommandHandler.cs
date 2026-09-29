using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Cart.Common;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Cart.Commands.RemoveCartItem
{
    public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, Result<CartResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;
        public RemoveCartItemCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<Result<CartResponse>> Handle(RemoveCartItemCommand request, CancellationToken ct)
        {
            var userId = _currentUser.Id;
            if (userId is null)
            {
                return Result<CartResponse>.Failure(NovaErrors.Unauthenticated);
            }
            var cartItem = await _context.CartItems
                                         .Include(c => c.Cart)
                                         .FirstOrDefaultAsync(ci => ci.Id == request.CartItemId &&
                                         ci.Cart!.UserId == userId.Value, ct);

            if (cartItem is null)
            {
                return Result<CartResponse>.Failure(NovaErrors.NotFound);
            }

            var cartId = cartItem.CartId;
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync(ct);
            var cart = await _context.Carts
                            .Include(c => c.CartItems)
                                .ThenInclude(p => p.Product)
                                    .ThenInclude(i => i.Images)
                            .FirstOrDefaultAsync(c => c.Id == cartId, ct);

            var response = new CartResponse
            {
                Id = cart.Id,
                Items = cart.CartItems?.Select(ci => new CartItemResponse
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name,
                    UnitPrice = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity,
                    ProductImage = ci.Product?.Images?
                        .OrderByDescending(i => i.IsMain)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                }).ToList() ?? []
            };

            return Result<CartResponse>.Success(response);
        }

    }

}