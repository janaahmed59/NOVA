using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Application.Features.Reviews.Common;
using NOVA.Application.Pagination;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Reviews.Queries.GetProductReview
{
    public class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQuery, Result<PagedResult<ReviewResponse>>>
    {
        private readonly IApplicationDbContext context;
        public GetProductReviewsQueryHandler(IApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Result<PagedResult<ReviewResponse>>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);
            if (product == null)
            {
                return Result<PagedResult<ReviewResponse>>.Failure(NovaErrors.NotFound);
            }

            // keep as IQueryable<ReviewResponse> so EF Core's CountAsync / Skip / Take / ToListAsync work
            var reviewsQuery = context.Reviews
                .Where(r => r.ProductId == request.ProductId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewResponse
                {
                    Id = r.Id,
                    ProductId = r.ProductId,
                    UserName = r.User.FullName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                });

            var totalCount = await reviewsQuery.CountAsync(cancellationToken);
            var pagedReviews = await reviewsQuery
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var result = new PagedResult<ReviewResponse>
            {
                Items = pagedReviews,
                TotalItems = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize),
                Page = request.Page,
                PageSize = request.PageSize
            };

            return Result<PagedResult<ReviewResponse>>.Success(result);
        }
    }
}
