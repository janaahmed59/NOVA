using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Cart.Common;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Entities;
namespace NOVA.Application.Features.Cart.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand, Result<CartResponse>>
    {
        private readonly IApplicationDbContext _Context;
        private readonly ICurrentUser _CurrentUser;
        public UpdateCartItemQuantityCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
        {
            _Context = context;
            _CurrentUser = currentUser;
        }
        public async Task<Result<CartResponse>> Handle(UpdateCartItemQuantityCommand request, CancellationToken ct)
        {
            var userId = _CurrentUser.Id;
            if (userId is null)
            {
                return Result<CartResponse>.Failure(NovaErrors.Unauthenticated);
            }

            var cartItem = await _Context.CartItems.Include(c => c.Cart)
                                                   .Include(p => p.Product)
                                                   .FirstOrDefaultAsync(ci => ci.Id == request.CartItemId
                                                    && ci.Cart!.UserId == userId.Value, ct); // ! -> 
            if (cartItem is null || cartItem.Product is null)
            {
                return Result<CartResponse>.Failure(NovaErrors.NotFound);
            }

            if (!cartItem.Product.IsActive)
            {
                return Result<CartResponse>.Failure(NovaErrors.ProductDeactivated);
            }
            if (cartItem.Product.StockQuantity < request.Quantity)
            {
                return Result<CartResponse>.Failure(
                    Error.Conflict("INSUFFICIENT_STOCK", $"Only {cartItem.Product.StockQuantity} items available in stock."));
            }

            cartItem.Quantity = request.Quantity;
            await _Context.SaveChangesAsync(ct);


            var cart = await _Context.Carts
                .Include(c => c.CartItems)!
                    .ThenInclude(ci => ci.Product)!
                        .ThenInclude(p => p.Images)
                .FirstAsync(c => c.Id == cartItem.CartId, ct);

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
