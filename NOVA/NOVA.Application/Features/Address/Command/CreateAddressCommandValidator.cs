using FluentValidation;
namespace NOVA.Application.Features.Address.Command
{
    public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressCommandValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .MaximumLength(250).WithMessage("Street cannot exceed 250 characters.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required.")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

            RuleFor(x => x.State)
                .MaximumLength(100).WithMessage("State cannot exceed 100 characters.");

            RuleFor(x => x.ZipCode)
                .MaximumLength(20).WithMessage("ZipCode cannot exceed 20 characters.");
        }
    }
}