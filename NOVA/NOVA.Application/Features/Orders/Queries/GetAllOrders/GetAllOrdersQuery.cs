using MediatR;
using NOVA.Application.Features.Orders.Common;
using NOVA.Application.Pagination;
using NOVA.Domain.Common.Results;
using NOVA.Domain.Enums;

namespace NOVA.Application.Features.Orders.Queries.GetAllOrders
{
    // يمكن للأدمن طلب صفحة معينة مع فلترة اختيارية بحالة الطلب (مثل: عرض الطلبات الـ Pending فقط)
    public record GetAllOrdersQuery(
        int Page = 1,
        int PageSize = 10,
        OrderStatus? Status = null
    ) : IRequest<Result<PagedResult<OrderResponse>>>;
}