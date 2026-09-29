using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Domain.Common.Results;


namespace NOVA.Application.Features.Products.Command.DeleteProduct
{
    public record DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context;
        public DeleteProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<Unit>> Handle(DeleteProductCommand request,CancellationToken ct)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.id, ct);
            if(product is null)
            {
                return Result<Unit>.Failure(NovaErrors.NotFound);
            }

            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return Result<Unit>.Success(Unit.Value);

        }
    }
}
