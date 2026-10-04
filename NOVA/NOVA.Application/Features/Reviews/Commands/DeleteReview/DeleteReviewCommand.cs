using MediatR;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Reviews.Commands.DeleteReview
{
    public record DeleteReviewCommand(int ReviewId) : IRequest<Result<Unit>>;
}
