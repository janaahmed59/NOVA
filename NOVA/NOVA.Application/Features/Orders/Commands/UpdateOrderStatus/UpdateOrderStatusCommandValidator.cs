using FluentValidation;

namespace NOVA.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("OrderId must be greater than zero.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid order status value.");
        }
    }
}