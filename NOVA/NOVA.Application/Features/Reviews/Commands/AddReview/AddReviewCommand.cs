using MediatR;
using NOVA.Application.Features.Reviews.Common;
using NOVA.Domain.Common.Results;

namespace NOVA.Application.Features.Reviews.Commands.AddReview
{
    public record AddReviewCommand(
        int ProductId,
        int Rating,
        string? Comment
    ) : IRequest<Result<ReviewResponse>>;
}
