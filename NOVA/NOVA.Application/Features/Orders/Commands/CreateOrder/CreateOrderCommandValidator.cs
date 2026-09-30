using FluentValidation;

namespace NOVA.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.AddressId)
                .GreaterThan(0).WithMessage("Valid shipping AddressId is required.");
        }
    }
}