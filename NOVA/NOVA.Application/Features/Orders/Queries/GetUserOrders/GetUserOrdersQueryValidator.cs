using FluentValidation;

namespace NOVA.Application.Features.Orders.Queries.GetUserOrders
{
    public class GetUserOrdersQueryValidator : AbstractValidator<GetUserOrdersQuery>
    {
        public GetUserOrdersQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
        }
    }
}