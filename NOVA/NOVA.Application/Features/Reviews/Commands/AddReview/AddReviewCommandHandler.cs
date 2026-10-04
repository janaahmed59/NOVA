using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Reviews.Common;
using NOVA.Domain.Common.Results;
using MediatR;
using NOVA.Application.Common.Errors;
using Microsoft.EntityFrameworkCore;
using NOVA.Domain.Entities;
namespace NOVA.Application.Features.Reviews.Commands.AddReview
{
    public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, Result<ReviewResponse>>
    {
        private readonly IApplicationDbContext applicationDbContext;
        private readonly ICurrentUser currentUser;
        public AddReviewCommandHandler(IApplicationDbContext applicationDbContext, ICurrentUser currentUser)
        {
            this.applicationDbContext = applicationDbContext;
            this.currentUser = currentUser;
        }
        public async Task<Result<ReviewResponse>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUser.Id;
            if(userId == null)
            {
                return Result<ReviewResponse>.Failure(NovaErrors.Unauthenticated);
            }
            var product = await applicationDbContext.Products
                                                    .FirstOrDefaultAsync(p => p.Id == request.ProductId && p.IsActive, cancellationToken);
            if(product == null)
            {
                return Result<ReviewResponse>.Failure(NovaErrors.NotFound);
            }
            var existingReview = await applicationDbContext.Reviews
                                                    .FirstOrDefaultAsync(r => r.ProductId == request.ProductId && r.UserId == userId, cancellationToken);
            if(existingReview is null)
            {
                return Result<ReviewResponse>.Failure(NovaErrors.ValidationError);
            }

            var review = new Review
            {
                ProductId = request.ProductId,
                UserId = userId.Value,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };
            applicationDbContext.Reviews.Add(review);
            await applicationDbContext.SaveChangesAsync(cancellationToken);
            return Result<ReviewResponse>.Success(new ReviewResponse
            {
                Id = review.Id,
                ProductId = review.ProductId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            });
        }
    }
}
