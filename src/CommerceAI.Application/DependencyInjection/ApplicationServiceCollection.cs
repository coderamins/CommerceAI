using CommerceAI.Application.Behaviors;
using CommerceAI.Application.Features.Products.CreateProduct;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CommerceAI.Application.DependencyInjection;

public static class ApplicationServiceCollection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

        return services;
    }
}
