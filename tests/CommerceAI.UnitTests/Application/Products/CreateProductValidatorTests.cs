using CommerceAI.Application.Features.Products.CreateProduct;
using FluentValidation.TestHelper;

namespace CommerceAI.UnitTests.Application.Products;

public class CreateProductValidatorTests
{
    private readonly CreateProductValidator _validator = new();

    [Fact]
    public void Should_have_error_when_name_is_empty()
    {
        var command = new CreateProductCommand(
                "",
                100,
                100
            );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor( x => x.Name ); 
    }

    [Fact]
    public void Should_have_error_when_price_is_zero()
    {
        var command = new CreateProductCommand(
                "p1",
                0,
                100
            );

        var result=_validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Should_have_error_when_stock_is_negative()
    {
        var command = new CreateProductCommand(
                "p2",
                100,
                -1
            );

        var result= _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Stock);
    }

    [Fact]
    public void Should_not_have_errors_for_valid_product()
    {
        var command = new CreateProductCommand(
            "Mechanical Keyboard",
            100,
            10);

        var result= _validator.TestValidate(command);   
        result.ShouldNotHaveAnyValidationErrors();  
    }
}
