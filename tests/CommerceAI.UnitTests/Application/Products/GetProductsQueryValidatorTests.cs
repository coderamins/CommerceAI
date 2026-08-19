using CommerceAI.Application.Features.Products.GetProducts;
using CommerceAI.Application.Queries.Products.GetProducts;
using FluentValidation.TestHelper;

namespace CommerceAI.UnitTests.Application.Products;

public class GetProductsQueryValidatorTests
{
    private readonly GetProductsQueryValidator _validator=new();

    [Fact]
    public void Should_have_error_when_page_size_is_greater_than_100()
    {
        var query = new GetProductsQuery(
                PageNumber: 1,
                PageSize: 98
            );

        var result =_validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x=>x.PageSize);
    }

    [Fact]
    public void Should_have_error_when_min_price_is_greater_than_max_price()
    {
        var query = new GetProductsQuery(
                PageNumber: 1,
                PageSize: 20,
                MinPrice: 500,
                MaxPrice: 100
            );

        var result=_validator.TestValidate(query);

        result.ShouldHaveValidationErrors();
    }

}
