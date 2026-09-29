using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Cart.Common;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Cart.Queries.GetCart
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, Result<CartResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public GetCartQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<Result<CartResponse>> Handle(GetCartQuery request, CancellationToken ct)
        {
            var userId = _currentUser.Id;
            if (userId is null)
            {
                return Result<CartResponse>.Failure(NovaErrors.Unauthenticated);
            }

            var cart = await _context.Carts.AsNoTracking()
                .Include(c => c.CartItems)!
                    .ThenInclude(ci => ci.Product)!
                        .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(c => c.UserId == userId.Value, ct);

            if (cart is null)
            {
                return Result<CartResponse>.Success(new CartResponse
                {
                    Id = 0,
                    Items = []
                });
            }

            var response = new CartResponse
            {
                Id = cart.Id,
                Items = cart.CartItems?
                    .Where(ci => ci.Product != null && ci.Product.IsActive)
                    .Select(ci => new CartItemResponse
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