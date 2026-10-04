using MediatR;
using Microsoft.EntityFrameworkCore;
using NOVA.Application.Common.Errors;
using NOVA.Application.Common.Interfaces;
using NOVA.Domain.Common.Results;
namespace NOVA.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUser user;
        public DeleteReviewCommandHandler(IApplicationDbContext context, ICurrentUser user)
        {
            this.context = context;
            this.user = user;
        }
        public async Task<Result<Unit>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var userId = user.Id;
            if(userId == null)
            {
                return Result<Unit>.Failure(NovaErrors.Unauthenticated);
            }
            var review = await context.Reviews.FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);
            if(review == null)
            {
                return Result<Unit>.Failure(NovaErrors.NotFound);
            }
            var userReview = await context.Reviews.FirstOrDefaultAsync(r => r.Id == request.ReviewId && r.UserId == userId, cancellationToken);
            if(userReview == null)
            {
                return Result<Unit>.Failure(NovaErrors.Forbidden);
            }

            context.Reviews.Remove(userReview);
            await context.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(Unit.Value);
        }

    }
}
