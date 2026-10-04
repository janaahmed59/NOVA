using NOVA.Application.Features.Reviews.Common;
using NOVA.Application.Pagination;
using MediatR;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Reviews.Queries.GetProductReview
{
    public record GetProductReviewsQuery(
        int ProductId,
        int Page,
        int PageSize
        ) : IRequest<Result<PagedResult<ReviewResponse>>>;

}
