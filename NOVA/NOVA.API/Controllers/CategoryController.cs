using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NOVA.API.Common.Responses;
using NOVA.Application.Features.Categories.Commands.Create_Category;
using NOVA.Application.Features.Categories.Commands.DeleteCategory;
using NOVA.Application.Features.Categories.Commands.UpdateCategory;
using NOVA.Application.Features.Categories.Queries.GetCategories;
using NOVA.Application.Features.Categories.Queries.GetCategoryById;
using NOVA.Domain.Common.Results;
using System.Linq.Expressions;

namespace NOVA.API.Controllers
{
    [Route("api/v1/categories")]
    public class CategoryController(ISender sender) : BaseApiController
    {
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id, CancellationToken ct)
        {
            var query = new GetCategoryByIdQuery(id);
            var res = await sender.Send(query, ct);
            return HandleResult(res, OkEnvelope);
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCategories([FromQuery] int page,
            [FromQuery] int pageSize ,CancellationToken ct)
        {
            var query = new GetCategoriesQuery(page, pageSize);
            var res = await sender.Send(query, ct);
            return HandlePagedResult(res);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command, CancellationToken ct)
        {
            var result = await sender.Send(command, ct);
            return HandleResult(result, CreatedEnvelope);
        }
        [HttpPut("id:int")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCategory([FromRoute] int Id ,[FromBody] UpdateCategoryRequest request, CancellationToken ct)
        {
            var command = new UpdateCategoryCommand
            (
                Id,
                request.Name,
                request.Description,
                request.ImageUrl
            );
            var res = await sender.Send(command, ct);
            return HandleResult(res, OkEnvelope);
        }
        [HttpDelete("id:int")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id, CancellationToken ct)
        {
            var command = new DeleteCategoryCommand(id);
            var res = await sender.Send(command, ct);
            return HandleNoContent(res);
        }


    }
}
