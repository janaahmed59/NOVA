using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NOVA.API.Common.Responses;
using NOVA.Application.Features.Orders.Commands.CreateOrder;
using NOVA.Application.Features.Orders.Commands.UpdateOrderStatus;
using NOVA.Application.Features.Orders.Common;
using NOVA.Application.Features.Orders.Queries.GetAllOrders;
using NOVA.Application.Features.Orders.Queries.GetOrderById;
using NOVA.Application.Features.Orders.Queries.GetUserOrders;
using NOVA.Domain.Enums;

namespace NOVA.API.Controllers
{
    [Route("api/v1/orders")]
    [Authorize] // يتطلب تسجيل الدخول
    public class OrderController(ISender sender) : BaseApiController
    {
        // 1. إتمام الشراء (Checkout)
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Checkout(
            [FromBody] CreateOrderCommand command,
            CancellationToken ct)
        {
            var result = await sender.Send(command, ct);
            return HandleResult(result, CreatedEnvelope);
        }

        // 2. استعراض طلبات المستخدم الحالي (Paged)
        [HttpGet("my-orders")]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrderResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyOrders(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var query = new GetUserOrdersQuery(page, pageSize);
            var result = await sender.Send(query, ct);
            return HandlePagedResult(result);
        }

        // 3. تفاصيل طلب معين (بالـ ID)
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(
            [FromRoute] int id,
            CancellationToken ct)
        {
            var query = new GetOrderByIdQuery(id);
            var result = await sender.Send(query, ct);
            return HandleResult(result, OkEnvelope);
        }

        // 4. استعراض كل طلبات المتجر (خاص بالأدمن فقط - Paged مع فلتر اختياري)
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrderResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllOrders(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] OrderStatus? status = null,
            CancellationToken ct = default)
        {
            var query = new GetAllOrdersQuery(page, pageSize, status);
            var result = await sender.Send(query, ct);
            return HandlePagedResult(result);
        }

        // 5. تعديل حالة الطلب (خاص بالأدمن فقط)
        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateOrderStatus(
            [FromRoute] int id,
            [FromBody] UpdateOrderStatusRequest request,
            CancellationToken ct)
        {
            var command = new UpdateOrderStatusCommand(id, request.Status);
            var result = await sender.Send(command, ct);
            return HandleNullData(result);
        }
    }
}