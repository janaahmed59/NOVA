using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NOVA.API.Common.Responses;
using NOVA.Application.Features.Products.Command.CreateProducts;
using NOVA.Application.Features.Products.Command.DeleteProduct;
using NOVA.Application.Features.Products.Command.UpdateProduct;
using NOVA.Application.Features.Products.Queries.GetProductById;
using NOVA.Application.Features.Products.Queries.GetPRoducts;

namespace NOVA.API.Controllers
{
    [Route("api/v1/product")]
    public class ProductController(ISender sender) : BaseApiController
    {
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById
            ([FromRoute] int id, CancellationToken ct)
        {
            var query = new GetProductByIdQuery(id);
            var res = await sender.Send(query, ct);

            return HandleResult(res, OkEnvelope);
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllProducts(
            CancellationToken ct,
            [FromQuery] int page,
            [FromQuery] int pageSize
            )
        {
            var query = new GetProductQuery(page, pageSize);
            var result = await sender.Send(query, ct);

            return HandlePagedResult(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command,CancellationToken ct)
        {
            var res = await sender.Send(command, ct);
            return HandleResult(res, CreatedEnvelope);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(
            [FromRoute] int id,
            CancellationToken ct)
        {
            var command = new DeleteProductCommand(id);

            var result = await sender.Send(command, ct);

            return HandleNullData(result);
        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<CreateProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(
            [FromRoute] int id,
            [FromBody] UpdateProductRequest request,
            CancellationToken ct)
        {
            var command = new UpdateProductCommand(
                id,
                request.Name,
                request.Description,
                request.Price,
                request.CategoryId,
                request.StockQuantity);

            var result = await sender.Send(command, ct);

            return HandleResult(result, OkEnvelope);
        }

    }
}
