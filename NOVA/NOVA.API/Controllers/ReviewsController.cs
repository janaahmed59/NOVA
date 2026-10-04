using MediatR;
using Microsoft.AspNetCore.Mvc;
using NOVA.API.Common.Responses;
using NOVA.Application.Features.Reviews.Commands.AddReview;
using NOVA.Application.Features.Reviews.Commands.DeleteReview;
using NOVA.Application.Features.Reviews.Common;
using NOVA.Application.Features.Reviews.Queries.GetProductReview;
using NOVA.Application.Pagination;
using NOVA.Domain.Common.Results;
namespace NOVA.API.Controllers
{
    [Route("api/[controller]")]
    public class ReviewsController(ISender sender) : BaseApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Result<PagedResult<ReviewResponse>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductReviews([FromRoute] int id, [FromQuery] int page, [FromQuery] int pageSize, CancellationToken ct)
        {
            var query = new GetProductReviewsQuery(id, page, pageSize);
            var result = await sender.Send(query, ct);
            return HandlePagedResult(result);
        }
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<Result<ReviewResponse>>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateReview(AddReviewCommand command, CancellationToken ct)
        {
            var result = await sender.Send(command, ct);
            return HandleResult(result, CreatedEnvelope);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteReview([FromRoute] int id, CancellationToken ct)
        {
            var command = new DeleteReviewCommand(id);
            var result = await sender.Send(command, ct);
            return HandleResult(result, OkEnvelope);
        }
    }
}
