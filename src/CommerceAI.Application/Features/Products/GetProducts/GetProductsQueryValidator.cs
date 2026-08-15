using CommerceAI.Application.Queries.Products.GetProducts;
using FluentValidation;

namespace CommerceAI.Application.Features.Products.GetProducts;

public class GetProductsQueryValidator:AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x)
            .Must(x =>
                !x.MinPrice.HasValue ||
                !x.MaxPrice.HasValue ||
                x.MinPrice <= x.MaxPrice)
            .WithMessage(
                "MinPrice must be less than or equal to MaxPrice.");
    }
}
