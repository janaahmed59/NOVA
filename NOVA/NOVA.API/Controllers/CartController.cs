using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NOVA.API.Common.Responses;
using NOVA.Application.Features.Cart.Commands.AddToCart;
using NOVA.Application.Features.Cart.Commands.RemoveCartItem;
using NOVA.Application.Features.Cart.Commands.UpdateCartItemQuantity;
using NOVA.Application.Features.Cart.Common;
namespace NOVA.API.Controllers
{
    [Route("api/v1/cart")]
    [Authorize]
    public class CartController(ISender sender) : BaseApiController
    {
        [HttpPost("items")]
        [ProducesResponseType(typeof(ApiResponse<CartResponse>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddToCart(
            [FromBody] AddItemToCartCommand command,
            CancellationToken ct)
        {
            var result = await sender.Send(command, ct);
            return HandleResult(result, CreatedEnvelope);
        }
        [HttpPut("items/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CartResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateCartItemQuantity(
            [FromRoute] int id,
            [FromBody] UpdateCartItemQuantityRequest request,
            CancellationToken ct)
        {
            var command = new UpdateCartItemQuantityCommand(id, request.Quantity);
            var result = await sender.Send(command, ct);
            return HandleResult(result, OkEnvelope);
        }
        [HttpDelete("items/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<CartResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveCartItem(
            [FromRoute] int id,
            CancellationToken ct)
        {
            var command = new RemoveCartItemCommand(id);
            var result = await sender.Send(command, ct);
            return HandleResult(result, OkEnvelope);
        }
    }
}
