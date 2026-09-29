using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace NOVA.Application.Features.Cart.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityValidator : AbstractValidator<UpdateCartItemQuantityCommand>
    {
        public UpdateCartItemQuantityValidator()
        {
            RuleFor(x => x.CartItemId)
                .GreaterThan(0).WithMessage("Valid CartItemId is required.");
            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");
        }
    }
}
